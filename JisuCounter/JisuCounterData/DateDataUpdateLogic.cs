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
        public List<DateData> Update(List<MS_WEEK>MsWeeks,int Year,int MsGakunenID)
        {
            List<DateData> ret = new List<DateData>();

            DateTime TargetDate = new DateTime(Year, 4, 1);
            DateTime FinishDate = TargetDate.AddYears(1);

            while (TargetDate < FinishDate)
            {
                //MS_WEEKからTargetDateの曜日のデータを取得する
                var YoubiDatas = MsWeeks.Where(x => x.DAY == (int)TargetDate.DayOfWeek);
                if (YoubiDatas != null)
                {
                    
                    foreach (var YoubiData in YoubiDatas)
                    {
                        DateData dateData = new DateData { JIKANWARI = TargetDate, KOMA = YoubiData.KOMA, MS_KYOUKA_ID = YoubiData.MS_KYOUKA_ID, MS_GAKUNEN_ID = MsGakunenID };
                        ret.Add(dateData);
                    }
                }
                TargetDate = TargetDate.AddDays(1);
            }
            return ret;
        }
    }
}
