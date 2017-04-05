using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace JisuCounterData
{
    public class DateDataController
    {
        public IList<DateData> Get(int MS_GAKUNEN_ID,int Year,int Month)
        {
            #region SQL
            string SQL = @"
select 
MS_GAKUNEN_ID,
JIKANWARI,
KOMA,
MS_KYOUKA_ID

from DATE_DATA
where MS_GAKUNEN_ID = :MS_GAKUNEN_ID
and strftime('%Y-%m',JIKANWARI) = :DATE
order by JIKANWARI,KOMA

";
            #endregion

            List<DateData> retDatas = new List<DateData>();

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}-{1}", Year, Month.ToString("D2"));

                command.Parameters.AddWithValue(":MS_GAKUNEN_ID", MS_GAKUNEN_ID);
                command.Parameters.AddWithValue(":DATE", sb.ToString());

                var reader = command.ExecuteReader();
                var mapper = new Mapper<DateData>();
                while (reader.Read())
                {
                    retDatas.Add(mapper.Mapping(reader));
                }
            }

            return retDatas;
        }

        public Dictionary<string,double>Get月時数(int Year,int Month)
        {
            #region SQL
            string SQL = @"
select MS_KYOUKA.KYOUKA_NAME,sum(MS_KYOUKA.KYOUKA_RATIO)

from DATE_DATA
inner join MS_KYOUKA on MS_KYOUKA.MS_KYOUKA_ID = DATE_DATA.MS_KYOUKA_ID

where strftime('%Y-%m',DATE_DATA.JIKANWARI) = '2017-04'
group by MS_KYOUKA.KYOUKA_NAME;
";
            #endregion

            Dictionary<string, double> retDatas = new Dictionary<string, double>();

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string key;
                    double sum;

                    key = reader.GetString(0);
                    sum = reader.GetDouble(1);
                    retDatas.Add(key, sum);
                }
            }

            return retDatas;
        }
    }
}
