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
    /// WeekDayEditWIndow.xaml の相互作用ロジック
    /// </summary>
    public partial class WeekDayEditWindow : Window
    {
        public WeekDayEditWindowData WeekDayEditWindowData = new WeekDayEditWindowData();

        public WeekDayEditWindow()
        {
            InitializeComponent();

            WeekDayEditWindowData.LoadKyoukaMaster();
            DataContext = WeekDayEditWindowData;
        }
    }
}
