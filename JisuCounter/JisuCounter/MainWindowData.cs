﻿using System;
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
        List<MS_JISU> m_Jisus = new List<MS_JISU>();

        Color FORE_COLOR = Colors.Black;

        bool IsModify = false;
        #region 年度選択
        int _SelectedYear = DateTime.Today.Year;
        public int SelectedYear
        {
            get
            {
                return _SelectedYear;
            }
            set
            {
                _SelectedYear = value;
                LoadDateData();
                RaisePropertyChanged("SelectedYear");
            }
        }
        #endregion

        public Grid CalenderGrid;

        private void Refresh()
        {
            if (CalenderGrid != null)
            {
                MakeCalender(CalenderGrid, SelectedYear, _SelectedMonth, new Func<List<DateData>, DateTime, List<DateData>>((x, jikanwari) =>
                {
                    Control.DayEditWindow window = new Control.DayEditWindow();
                    window.DayEditWindowData.MS_GAKUNEN_ID = SelectedMsGakunen.MS_GAKUNEN_ID;
                    window.DayEditWindowData.DateDatas = x;
                    window.DayEditWindowData.Jikanwari = jikanwari;
                    if (window.ShowDialog() == true)
                    {
                        IsModify = true;
                        MakeYearSum();
                        return window.DayEditWindowData.DateDatas;

                    }
                    return null;
                }));
            }
        }
        
        #region 月選択
        int _SelectedMonth { get; set; }
        public int SelectedMonth {
            get
            {
                return _SelectedMonth;
            }
            set
            {
                _SelectedMonth = value;
                RaisePropertyChanged("SelectedMonth");

                Refresh();
            }
        }

        internal void BulkUpdate(List<MS_WEEK> msWeeks)
        {
            ///一括設定
            DateDataUpdateLogic logic = new DateDataUpdateLogic();


            logic.Update(m_DateDatas,msWeeks, SelectedYear, SelectedMonth, SelectedMsGakunen.MS_GAKUNEN_ID);

            Refresh();
        }
        #endregion

        #region 月のCombobox
        public ObservableCollection<int> MonthItems
        {
            get
            {
                ObservableCollection<int> rets = new ObservableCollection<int>();

                rets.Add(4);
                rets.Add(5);
                rets.Add(6);
                rets.Add(7);
                rets.Add(8);
                rets.Add(9);
                rets.Add(10);
                rets.Add(11);
                rets.Add(12);
                rets.Add(1);
                rets.Add(2);
                rets.Add(3);

                return rets;

            }
        }
        #endregion

        MS_GAKUNEN _SelectedMsGakunen { get; set; }
        public MS_GAKUNEN SelectedMsGakunen
        {
            get { return _SelectedMsGakunen; }
            set
            {
                if (IsModify == true)
                {
                    if (System.Windows.MessageBox.Show("データが変更されています。保存しますか？", "保存", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                    {
                        Save();
                    }
                }
                IsModify = false;
                _SelectedMsGakunen = value;
                LoadDateData();


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
            DBConnect.GetInstance().Open(@"Database=Jisu;Data Source=jisudb.cjzvzzwypzj9.us-east-2.rds.amazonaws.com;User Id=JisuAccessUser;Password=ZU6CzwL7lNgvXv");
            MS_KYOUKA_CACHE.CacheFill();
            

        }

        void IDisposable.Dispose()
        {
            DBConnect.GetInstance().Dispose();
        }

        public void LoadData()
        {
            SelectedYear = 2017;
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

        public void LoadDateData()
        {

            DateDataController DateDataController = new DateDataController();

            m_DateDatas = DateDataController.Get(SelectedMsGakunen.MS_GAKUNEN_ID, SelectedYear);

            LoadJisu();

            MakeYearSum();
            Refresh();
        }
        public void LoadJisu()
        {
            MS_JISU_Controller MsJisuController = new MS_JISU_Controller();
            m_Jisus = MsJisuController.GetAt(SelectedMsGakunen);
        }

        public void MakeCalender(Grid CalenderGrid,int year, int month ,Func<List<DateData>,DateTime, List<DateData>> ClickAction)
        {

            CalenderGrid.Children.Clear();
            m_DayControls.Clear();

            if (month < 4)
            {
                year++;
            }
            try
            {
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
                    dayControl.ClickAction = new Action<List<DateData>>(x =>
                    {
                        var EditDatas = ClickAction(x, date);
                        if (EditDatas != null)
                        {
                            m_DateDatas.RemoveAll(d => d.JIKANWARI.ToString("yyyyMMdd") == date.Year.ToString() + date.Month.ToString("D2") + date.Day.ToString("D2"));

                            foreach (var EditData in EditDatas)
                            {
                                
                                var a = m_DateDatas.Where(d => d.JIKANWARI == EditData.JIKANWARI && d.KOMA == EditData.KOMA).FirstOrDefault();
                                if (a == null)
                                {
                                    m_DateDatas.Add(EditData);
                                }
                            }
                            ///編集した1日データを更新
                            dayControl.Refresh();
                            ///月合計を更新
                            MakeMonthSum();
                            ///年合計を更新
                            MakeYearSum();
                        }
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
                ///月合計を更新
                MakeMonthSum();
                ///年合計を更新
                MakeYearSum();
            }
            catch { }
        }

        /// <summary>
        /// 合計時数の幅
        /// </summary>
        double JISU_WIDTH = 40d;

        #region 月合計
        List<MonthSumLabel> MonthSumLabels = new List<MonthSumLabel>();

        class MonthSumLabel
        {
            internal Label label;
            internal Label SumLabel;

        }

        /// <summary>
        /// 月合計
        /// </summary>
        MonthSumLabel MonthlySum;

        public void MakeMonthSumBase(Panel MonthSum)
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
            var a = MakeSumLabel(("合計", "Transparent"));
            MonthSumLabels.Add(a.SumLabels);
            MonthSum.Children.Add(a.Grid);
            MonthlySum = a.SumLabels;
        }

        (MonthSumLabel SumLabels,Grid Grid) MakeSumLabel((string KYOUKA_NAME, string COLOR)labelData)
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
            SumLabel.Width = JISU_WIDTH;
            grid.Children.Add(SumLabel);

            MonthSumLabel sumLabels = new MonthSumLabel { label = label, SumLabel = SumLabel };
            return (sumLabels,grid);
            

        }


        /// <summary>
        /// 次ごとの教科別時数合計
        /// </summary>
        public void MakeMonthSum()
        {
            DateDataController DateDataController = new DateDataController();
            var datas = DateDataController.Get月時数(m_DateDatas, SelectedYear, SelectedMonth);

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
            }

            ///月合計を計算
            MonthlySum.SumLabel.Content = DateDataController.月合計(m_DateDatas, SelectedYear, SelectedMonth);
        }

        #endregion

        #region 年合計
        class YearSumLabel
        {
            internal Label label;
            internal Label SumLabel;
            /// <summary>
            /// 年で決められている時数
            /// </summary>
            internal Label JisuLabel;
        }
        List<YearSumLabel> YearSumLabels = new List<YearSumLabel>();

        YearSumLabel SumLabel;
        public void MakeYearSumBase(Panel YearSum)
        {
            MS_KYOUKA_Controller MsKyoukaController = new MS_KYOUKA_Controller();
            

            var Kyoukas = MsKyoukaController.Get教科一覧();

            foreach (var kyouka in Kyoukas)
            {
                var l = MakeYearSumLabel(kyouka);
                YearSumLabels.Add(l.SumLabels);
                YearSum.Children.Add(l.Grid);
            }

            ///月の合計を追加
            var a = MakeYearSumLabel(("合計", "Transparent"));
            YearSumLabels.Add(a.SumLabels);
            SumLabel = a.SumLabels;
            YearSum.Children.Add(a.Grid);

        }

        (YearSumLabel SumLabels, Grid Grid) MakeYearSumLabel((string KYOUKA_NAME, string COLOR) labelData)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Label label = new Label();
            label.SetValue(Grid.ColumnProperty, 0);
            label.Content = labelData.KYOUKA_NAME;

            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(labelData.COLOR);

            label.Foreground = new SolidColorBrush(FORE_COLOR);
            label.Background = new SolidColorBrush(color);
            grid.Children.Add(label);

            Label JisuLabel = new Label();
            JisuLabel.SetValue(Grid.ColumnProperty, 1);
            JisuLabel.Content = "0";
            JisuLabel.Width = JISU_WIDTH;
            grid.Children.Add(JisuLabel);

            Label SumLabel = new Label();
            SumLabel.SetValue(Grid.ColumnProperty, 2);
            SumLabel.Content = "0";
            SumLabel.Width = JISU_WIDTH;
            grid.Children.Add(SumLabel);


            YearSumLabel sumLabels = new YearSumLabel { label = label, SumLabel = SumLabel ,JisuLabel = JisuLabel };
            return (sumLabels, grid);


        }

        /// <summary>
        /// 次ごとの教科別時数合計(年)
        /// </summary>
        public void MakeYearSum()
        {
            DateDataController DateDataController = new DateDataController();
            var datas = DateDataController.Get年時数(m_DateDatas);

            double SumJisu = 0d;
            //一度合計値を０にする
            foreach (var control in YearSumLabels)
            {
                control.SumLabel.Content = 0;
                var jisu = m_Jisus.Where(x => x.MS_GAKUNEN_ID == SelectedMsGakunen.MS_GAKUNEN_ID && x.KYOUKA_NAME == (string)control.label.Content).FirstOrDefault();
                if (jisu != null)
                {
                    control.JisuLabel.Content = jisu.JISU;

                    if (datas.ContainsKey((string)control.label.Content))
                    {
                        var 年時数 = datas[(string)control.label.Content];
                        if (jisu.JISU > 年時数)
                        {
                            control.SumLabel.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            control.SumLabel.Foreground = new SolidColorBrush(FORE_COLOR);
                        }
                        control.SumLabel.Content = 年時数;
                    }
                    else
                    {
                        if (jisu.JISU > 0)
                        {
                            control.SumLabel.Foreground = new SolidColorBrush(Colors.Red);
                        }
                        else
                        {
                            control.SumLabel.Foreground = new SolidColorBrush(FORE_COLOR);
                        }
                    }
                }
                else
                {
                    control.JisuLabel.Content = "0";
                }
            }

            var 全時数合計 = m_DateDatas.Join(MS_KYOUKA_CACHE.GetAll(), x => x.MS_KYOUKA_ID, j => j.MS_KYOUKA_ID, (x, j) => new
            {
                MS_KYOUKA_ID = j.MS_KYOUKA_ID,
                RATIO = j.KYOUKA_RATIO
            });

            
            if (SumLabel != null)
            {
                SumLabel.JisuLabel.Content = m_Jisus.Sum(x => x.JISU);
                SumLabel.SumLabel.Content = 全時数合計.Sum(x => x.RATIO);
                if (全時数合計.Sum(x => x.RATIO) < m_Jisus.Sum(x => x.JISU) )
                {
                    SumLabel.SumLabel.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    SumLabel.SumLabel.Foreground = new SolidColorBrush(FORE_COLOR);
                }
            }

        }

        #endregion

        public void Save()
        {
            DateDataController DateDataController = new DateDataController();

            DateDataController.Save(m_DateDatas,_SelectedMsGakunen, SelectedYear);
        }
    }
}
