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
using JisuCounterData;

namespace JisuCounter.JisuMaster
{
    /// <summary>
    /// JisuMasterWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class JisuMasterWindow : Window
    {
        JisuMasterWindowData JisuMasterWindowData = new JisuMasterWindowData();

        public MS_GAKUNEN TargetGakunen
        {
            get { return JisuMasterWindowData.TargetGakunenID; }
            set { JisuMasterWindowData.TargetGakunenID = value; }
        }

        public List<MS_JISU> TargetJisus
        {
            get
            {
                return JisuMasterWindowData.m_Jisus;
            }
        }
        public JisuMasterWindow()
        {
            InitializeComponent();

            DataContext = JisuMasterWindowData;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            JisuMasterWindowData.Load(YearSumPanel);
        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            JisuMasterWindowData.SetYearData();

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
