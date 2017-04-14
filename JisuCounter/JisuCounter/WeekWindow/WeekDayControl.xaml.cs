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

namespace JisuCounter.WeekWindow
{
    /// <summary>
    /// WeekDayControl.xaml の相互作用ロジック
    /// </summary>
    public partial class WeekDayControl : UserControl
    {
        public WeekDayControlData weekDayControlData = new WeekDayControlData();

        public string Day
        {
            get { return weekDayControlData.Day; }
            set { weekDayControlData.Day = value; }
        }
        public int iDay;

        public Action<List<JisuCounterData.MS_WEEK>,int> ClickAction;

        public WeekDayControl()
        {
            InitializeComponent();

            DataContext = weekDayControlData;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ClickAction?.Invoke(weekDayControlData.MsWeeks,iDay);
        }
        public void Refresh()
        {
            weekDayControlData.Refresh();
        }
    }
}
