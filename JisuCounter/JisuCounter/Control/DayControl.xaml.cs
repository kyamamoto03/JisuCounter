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

namespace JisuCounter.Control
{
    /// <summary>
    /// DayControl.xaml の相互作用ロジック
    /// </summary>
    public partial class DayControl : UserControl
    {
        public DayControlData DayControlData;
        public Action<List<JisuCounterData.DateData>> ClickAction;

        public DayControl()
        {
            InitializeComponent();
            DayControlData = new DayControlData();
            DataContext = DayControlData;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickAction?.Invoke(DayControlData.DateDatas);
        }

        public void Refresh()
        {
            DayControlData.Refresh();
        }
    }
}
