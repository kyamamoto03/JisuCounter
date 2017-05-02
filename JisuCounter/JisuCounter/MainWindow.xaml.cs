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
using Microsoft.Win32;

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
            DataContext = mainWindowData;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FilterIndex = 1;
            openFileDialog.Filter = "字数 ファイル(.jisu)|*.jisu";
            bool? result = openFileDialog.ShowDialog();
            if (result == false)
            {
                MessageBox.Show("プログラムを終了します");
                Close();
                return;
            }

            mainWindowData.DBOpen(openFileDialog.FileName);

            mainWindowData.LoadMaster();

            mainWindowData.LoadData();



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
            MessageBox.Show("更新しました", "更新", MessageBoxButton.OK);
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

        private void JisuSetButton_Click(object sender, RoutedEventArgs e)
        {
            JisuMaster.JisuMasterWindow window = new JisuMaster.JisuMasterWindow();
            window.TargetGakunen = mainWindowData.SelectedMsGakunen;

            if (window.ShowDialog() == true)
            {
                ///字数を更新する
                MS_JISU_Controller JisuController = new MS_JISU_Controller();

                JisuController.Inserts(mainWindowData.SelectedMsGakunen, window.TargetJisus);
                mainWindowData.LoadDateData();
            }
        }
    }
}
