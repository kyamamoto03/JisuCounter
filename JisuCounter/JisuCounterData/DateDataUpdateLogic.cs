using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JisuCounterData
{
    /// <summary>
    /// 一週間のデータで一年を書き換える
    /// </summary>
    public class DateDataUpdateLogic
    {
        public void Update(List<DateData> DateDatas,List<MS_WEEK>MsWeeks,int Year,int Month,int MsGakunenID)
        {

            int TargetYear = Year;
            if (Month < 4)
            {
                TargetYear++;
            }
            DateTime TargetDate = new DateTime(TargetYear, Month, 1);
            DateTime FinishDate = new DateTime(TargetYear, Month, DateTime.DaysInMonth(Year, Month));

            //ターゲットのデータを削除する
            DateDatas.RemoveAll(x =>
            {
                if (x.MS_GAKUNEN_ID == MsGakunenID && x.JIKANWARI >= TargetDate && x.JIKANWARI <= FinishDate)
                    return true;
                else
                    return false;
            });

            while (TargetDate < FinishDate)
            {
                //MS_WEEKからTargetDateの曜日のデータを取得する
                var YoubiDatas = MsWeeks.Where(x => x.DAY == (int)TargetDate.DayOfWeek);
                if (YoubiDatas != null )
                {

                    foreach (var YoubiData in YoubiDatas)
                    {
                        if (YoubiData.MS_KYOUKA_ID > 0)
                        {
                            DateData dateData = new DateData { JIKANWARI = TargetDate, KOMA = YoubiData.KOMA, MS_KYOUKA_ID = YoubiData.MS_KYOUKA_ID, MS_GAKUNEN_ID = MsGakunenID };
                            DateDatas.Add(dateData);
                        }
                    }
                }
                TargetDate = TargetDate.AddDays(1);
            }
        }
    }
}
