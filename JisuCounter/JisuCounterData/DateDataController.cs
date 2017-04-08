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
        public List<DateData> Get(int MS_GAKUNEN_ID,int Year,int Month)
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

        public Dictionary<string,double>Get月時数(List<DateData> dateData)
        {
            Dictionary<string, double> retDatas = new Dictionary<string, double>();

            var joinDatas = dateData.Join(MS_KYOUKA_CACHE.GetAll(), x => x.MS_KYOUKA_ID, j => j.MS_KYOUKA_ID, (x, j) => new
            {
                KYOUKA_NAME = j.KYOUKA_NAME,
                RATIO = j.KYOUKA_RATIO
            });
            var groupDatas = joinDatas.GroupBy(x => x.KYOUKA_NAME).Select(a => new { KYOUKA_NAME = a.Key, Sum = a.Sum(x => x.RATIO) });

            foreach(var d in groupDatas)
            {
                retDatas.Add(d.KYOUKA_NAME, d.Sum);
            }
            return retDatas;
        }

        double ROUND = 10.0;

        public double 月合計(List<DateData> dateData)
        {
            var joinDatas = dateData.Join(MS_KYOUKA_CACHE.GetAll(), x => x.MS_KYOUKA_ID, j => j.MS_KYOUKA_ID, (x, j) => new
            {
                KYOUKA_NAME = j.KYOUKA_NAME,
                RATIO = j.KYOUKA_RATIO
            });

            return (int)(joinDatas.Sum(x => x.RATIO) * ROUND) / ROUND;
        }

        public void Save(List<DateData> dateDatas, MS_GAKUNEN Gakunen, int Year, int Month)
        {
            using (SQLiteTransaction trans = DBConnect.GetConnection().BeginTransaction())
            {
                DeleteYearMonth(Gakunen, Year, Month);

                foreach(var d in dateDatas)
                {
                    Insert(d);
                }
                trans.Commit();
            }
        }

        private void Insert(DateData dateData)
        {
            #region SQL 
            string SQL = @"
insert into DATE_DATA(
MS_GAKUNEN_ID,
JIKANWARI,
KOMA,
MS_KYOUKA_ID)
values(
:MS_GAKUNEN_ID,
:JIKANWARI,
:KOMA,
:MS_KYOUKA_ID)

";
            #endregion
            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                command.Parameters.AddWithValue(":MS_GAKUNEN_ID", dateData.MS_GAKUNEN_ID);
                command.Parameters.AddWithValue(":JIKANWARI", dateData.JIKANWARI);
                command.Parameters.AddWithValue(":KOMA", dateData.KOMA);
                command.Parameters.AddWithValue(":MS_KYOUKA_ID", dateData.MS_KYOUKA_ID);

                command.ExecuteNonQuery();
            }

        }
        private void DeleteYearMonth(MS_GAKUNEN Gakunen, int Year, int Month)
        {
            #region SQL 
            string SQL = @"
delete from DATE_DATA
where MS_GAKUNEN_ID = :MS_GAKUNEN_ID
and strftime('%Y-%m', JIKANWARI) = :JIKANWARI
";
            #endregion
            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}-{1}", Year, Month.ToString("D2"));

                command.Parameters.AddWithValue(":MS_GAKUNEN_ID", Gakunen.MS_GAKUNEN_ID);
                command.Parameters.AddWithValue(":JIKANWARI", sb.ToString());

                command.ExecuteNonQuery();
            }
        }
    }
}
