using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JisuCounterData;

namespace JisuCounter
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowData mainWindowData;

         public MainWindow()
        {
            InitializeComponent();
            mainWindowData = new MainWindowData();
            mainWindowData.DBOpen();

            mainWindowData.LoadMaster();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mainWindowData.LoadData();


            DataContext = mainWindowData;

            mainWindowData.MakeMonthSumBase(MonthSum);
            mainWindowData.MakeYearSumBase(YearSum);


            mainWindowData.MakeMonthSum();
            mainWindowData.MakeYearSum();

            mainWindowData.CalenderGrid = CalenderGrid;

            mainWindowData.SelectedYear = 2017;
            mainWindowData.SelectedMonth = 4;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindowData.Save();
        }

        /// <summary>
        /// 一括設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeekWindowButton_Click(object sender, RoutedEventArgs e)
        {
            WeekWindow.WeekWindow weekWindow = new WeekWindow.WeekWindow();

            if (weekWindow.ShowDialog() == true)
            {
                mainWindowData.BulkUpdate(weekWindow.weekWindowData.MsWeeks);
            }

        }
    }
}
