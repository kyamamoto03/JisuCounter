using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using JisuCounter.Control;
using JisuCounterData;
using System.Windows.Media;

namespace JisuCounter
{
    class MainWindowData : IDisposable, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string PropertyName)
        {
            var d = PropertyChanged;
            if (d != null)
            {
                d(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion

        public ObservableCollection<MS_GAKUNEN> MsGakunen { get; set; }
        public ObservableCollection<MS_KYOUKA> MsKyoukas { get; set; }

        List<DayControl> m_DayControls = new List<DayControl>();
        List<DateData> m_DateDatas = new List<DateData>();

        Color FORE_COLOR = Colors.White;

        MS_GAKUNEN _SelectedMsGakunen { get; set; }
        public MS_GAKUNEN SelectedMsGakunen
        {
            get { return _SelectedMsGakunen; }
            set
            {
                _SelectedMsGakunen = value;
                RaisePropertyChanged("SelectedMsGakunen");
            }
        }

        public MainWindowData()
        {
            MsKyoukas = new ObservableCollection<JisuCounterData.MS_KYOUKA>();
            MsGakunen = new ObservableCollection<MS_GAKUNEN>();

        }

        public void DBOpen()
        {
            DBConnect.GetInstance().Open(@"Data Source=JisuCounter.sqlite3");
            MS_KYOUKA_CACHE.CacheFill();

        }

        void IDisposable.Dispose()
        {
            DBConnect.GetInstance().Dispose();
        }

        public void LoadMaster()
        {
            MS_KYOUKA_Controller MsKyoukaController = new MS_KYOUKA_Controller();


            var Kyoukas = MsKyoukaController.GetAll();
            MsKyoukas.Clear();
            foreach(var kyouka in Kyoukas)
            {
                MsKyoukas.Add(kyouka);
            }

            MS_GAKUNEN_Controller MsGakunenController = new MS_GAKUNEN_Controller();
            var Gakunens = MsGakunenController.GetAll();
            MsGakunen.Clear();
            foreach(var gakunen in Gakunens)
            {
                MsGakunen.Add(gakunen);
            }

            SelectedMsGakunen = MsGakunen[0];
        }

        void LoadDateData(int year,int month)
        {
            DateDataController DateDataController = new DateDataController();

            m_DateDatas = DateDataController.Get(1, year, month);
        }


        public void MakeCalender(Grid CalenderGrid,int year, int month ,Func<List<JisuCounterData.DateData>,DateTime, JisuCounterData.DateData> ClickAction)
        {
            LoadDateData(year, month);

            DateTime ThisMonth = new DateTime(year, month, 1);

            int YoubiIndex = (int)ThisMonth.DayOfWeek;
            int Row = 0;
            int Col = YoubiIndex;
            for (int i = 1; i <= DateTime.DaysInMonth(ThisMonth.Year, ThisMonth.Month); i++)
            {
                DayControl dayControl = new DayControl();
                dayControl.SetValue(Grid.RowProperty, Row);
                dayControl.SetValue(Grid.ColumnProperty, Col++);
                DateTime date = new DateTime(ThisMonth.Year, ThisMonth.Month, i);

                dayControl.DayControlData.Add(m_DateDatas.Where(x => x.JIKANWARI.ToString("yyyyMMdd") == ThisMonth.Year.ToString() + ThisMonth.Month.ToString("D2") + i.ToString("D2")));
                dayControl.ClickAction = new Action<List<JisuCounterData.DateData>>(x =>
                {
                    var EditData = ClickAction(x, date);
                    var a = m_DateDatas.Where(d => d.JIKANWARI == EditData.JIKANWARI).FirstOrDefault();
                    if (a == null)
                    {
                        m_DateDatas.Add(EditData);
                    }
                    dayControl.Refresh();
                    MakeMonthSum();
                });


                CalenderGrid.Children.Add(dayControl);

                dayControl.DayControlData.Day = new DateTime(ThisMonth.Year, ThisMonth.Month, i);

                m_DayControls.Add(dayControl);

                if (Col > 6)
                {
                    Col = 0;
                    Row++;
                }
            }
        }

        List<SumLabels> MonthSumLabels = new List<SumLabels>();

        class SumLabels
        {
            internal Label label;
            internal Label SumLabel;

        }

        /// <summary>
        /// 月合計
        /// </summary>
        SumLabels MonthlySum;

        public void MakeMonthSumBase(StackPanel MonthSum)
        {
            MS_KYOUKA_Controller MsKyoukaController = new MS_KYOUKA_Controller();
            //MonthSum
            var Kyoukas = MsKyoukaController.Get教科一覧();

            foreach(var kyouka in Kyoukas)
            {
                var l = MakeSumLabel(kyouka);
                MonthSumLabels.Add(l.SumLabels);
                MonthSum.Children.Add(l.Grid);
            }

            ///月の合計を追加
            var a = MakeSumLabel(("合計","Black"));
            MonthSumLabels.Add(a.SumLabels);
            MonthSum.Children.Add(a.Grid);
            MonthlySum = a.SumLabels;
        }

        (SumLabels SumLabels,Grid Grid) MakeSumLabel((string KYOUKA_NAME, string COLOR)labelData)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Label label = new Label();
            label.SetValue(Grid.ColumnProperty, 0);
            label.Content = labelData.KYOUKA_NAME;

            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(labelData.COLOR);

            label.Foreground = new SolidColorBrush(FORE_COLOR);
            label.Background = new SolidColorBrush(color);
            grid.Children.Add(label);

            Label SumLabel = new Label();
            SumLabel.SetValue(Grid.ColumnProperty, 1);
            SumLabel.Content = "0";
            grid.Children.Add(SumLabel);

            SumLabels sumLabels = new SumLabels { label = label, SumLabel = SumLabel };
            return (sumLabels,grid);
            

        }
 
        /// <summary>
        /// 次ごとの教科別時数合計
        /// </summary>
        public void MakeMonthSum()
        {
            DateDataController DateDataController = new DateDataController();
            var datas = DateDataController.Get月時数(m_DateDatas);

            //一度合計値を０にする
            foreach (var control in MonthSumLabels)
            {
                control.SumLabel.Content = 0;
            }

            foreach (var data in datas)
            {
                var targetLabels = MonthSumLabels.Where(x => (string)x.label.Content == data.Key).FirstOrDefault();
                if (targetLabels != null)
                {
                    targetLabels.SumLabel.Content = data.Value;
                }
                else
                {
                    targetLabels.SumLabel.Content = 0;
                }
            }

            ///月合計を計算
            MonthlySum.SumLabel.Content = DateDataController.月合計(m_DateDatas);
        }

        public void Save()
        {
            DateDataController DateDataController = new DateDataController();

            DateDataController.Save(m_DateDatas,_SelectedMsGakunen, 2017,4);
        }
    }
}
