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

namespace JisuCounter.Control
{
    /// <summary>
    /// DayEditWIndow.xaml の相互作用ロジック
    /// </summary>
    public partial class DayEditWindow : Window
    {
        public DayEditWindowData DayEditWindowData = new DayEditWindowData();
        public DayEditWindow()
        {
            InitializeComponent();

            DayEditWindowData.LoadKyoukaMaster();
            DataContext = DayEditWindowData;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DayEditWindowData.RollBack();
            DialogResult = false;
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DayEditWindowData.Init();
        }
    }
}
