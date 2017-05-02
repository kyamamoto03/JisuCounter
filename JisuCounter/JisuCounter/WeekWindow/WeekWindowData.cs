using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using JisuCounter.Control;
using JisuCounterData;

namespace JisuCounter.WeekWindow
{
    public class WeekWindowData
    {
        public List<MS_WEEK> MsWeeks = new List<MS_WEEK>();

        public void LoadMsWeeks()
        {
            MS_WEEK_Controller MsWeekController = new MS_WEEK_Controller();
            MsWeeks = MsWeekController.Get();
        }

        public void SetDayControl(StackPanel DayControlStackPanel)
        {
            string[] DayNmae = { "日", "月", "火", "水", "木", "金", "土" };

            for (int i = 0; i < 7; i++)
            {
                WeekDayControl weekDayControl = new WeekDayControl();
                weekDayControl.Day = DayNmae[i];
                weekDayControl.iDay = i;

                weekDayControl.weekDayControlData.Add(MsWeeks.Where(x => x.DAY == i));

                weekDayControl.ClickAction = new Action<List<MS_WEEK>,int>((x,day) =>
                {
                    WeekDayEditWindow window = new WeekDayEditWindow();
                    window.WeekDayEditWindowData.MsWeeks = x;
                    window.WeekDayEditWindowData.Day = day;
                    window.ShowDialog();

                    var EditDatas = window.WeekDayEditWindowData.MsWeeks;
                    MsWeeks.RemoveAll(w => w.DAY == day);

                    foreach (var EditData in EditDatas)
                    {

                        

                        var a = MsWeeks.Where(d => d.DAY == EditData.DAY && d.KOMA == EditData.KOMA).FirstOrDefault();
                        if (a == null && EditData.MS_KYOUKA_ID > 0)
                        {
                            MsWeeks.Add(EditData);
                        }
                    }
                    ///編集した1日データを更新
                    weekDayControl.Refresh();

                });


                DayControlStackPanel.Children.Add(weekDayControl);

            }

        }

        public void MsWeeksUpdate()
        {
            MS_WEEK_Controller MsWeekController = new MS_WEEK_Controller();
            MsWeekController.Updates(MsWeeks);


        }
    }
}
