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
        IList<DateData> m_DateDatas = new List<DateData>();

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
        }

        void LoadDateData(int year,int month)
        {
            DateDataController DateDataController = new DateDataController();

            m_DateDatas = DateDataController.Get(1, year, month);
        }


        public void MakeCalender(Grid CalenderGrid,int year, int month ,Action<List<JisuCounterData.DateData>> ClickAction)
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
                dayControl.DayControlData.Add(m_DateDatas.Where(x => x.JIKANWARI.ToString("yyyyMMdd") == ThisMonth.Year.ToString() + ThisMonth.Month.ToString("D2") + i.ToString("D2")));
                dayControl.ClickAction = new Action<List<JisuCounterData.DateData>>(x =>
                {
                    ClickAction(x);
                    dayControl.Refresh();
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
        public void MakeMonthSumBase(StackPanel MonthSum)
        {
            MS_KYOUKA_Controller MsKyoukaController = new MS_KYOUKA_Controller();
            //MonthSum
            var Kyoukas = MsKyoukaController.Get教科一覧();

            foreach(var kyouka in Kyoukas)
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                Label label = new Label();
                label.SetValue(Grid.ColumnProperty, 0);
                label.Content = kyouka;
                grid.Children.Add(label);

                Label SumLabel = new Label();
                SumLabel.SetValue(Grid.ColumnProperty, 1);
                SumLabel.Content = "0";
                grid.Children.Add(SumLabel);

                MonthSumLabels.Add(new SumLabels { label = label, SumLabel = SumLabel });

                MonthSum.Children.Add(grid);
            }
        }
        public void MakeMonthSum(int Year,int Month)
        {
            DateDataController DateDataController = new DateDataController();
            var datas = DateDataController.Get月時数(Year,Month);

            foreach(var data in datas)
            {
                var targetLabels = MonthSumLabels.Where(x => (string)x.label.Content == data.Key).FirstOrDefault();
                if (targetLabels != null)
                {
                    targetLabels.SumLabel.Content = data.Value;
                }
            }
        }
    }
}
