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
using System.Windows.Shapes;

namespace JisuCounter.WeekWindow
{
    /// <summary>
    /// WeekWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class WeekWindow : Window
    {
        public WeekWindowData weekWindowData = new WeekWindowData();
        public WeekWindow()
        {
            InitializeComponent();

            weekWindowData.LoadMsWeeks();

            weekWindowData.SetDayControl(DayControlStackPanel);
            DataContext = weekWindowData;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            ///曜日マスタ更新
            try
            {
                weekWindowData.MsWeeksUpdate();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DialogResult = true;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
