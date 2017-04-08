﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace JisuCounterData
{
    public class DateDataController
    {
        public List<DateData> Get(int MS_GAKUNEN_ID,int Year)
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
and strftime('%Y-%m', JIKANWARI) >= :NENDO_START and strftime('%Y-%m', JIKANWARI) <= :NENDO_END
order by JIKANWARI,KOMA

";
            #endregion

            List<DateData> retDatas = new List<DateData>();

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                command.Parameters.AddWithValue(":MS_GAKUNEN_ID", MS_GAKUNEN_ID);
                var 年度 = Get年度(Year);

                command.Parameters.AddWithValue(":NENDO_START", 年度.NendoStart);
                command.Parameters.AddWithValue(":NENDO_END", 年度.NendoEnd);

                var reader = command.ExecuteReader();
                var mapper = new Mapper<DateData>();
                while (reader.Read())
                {
                    retDatas.Add(mapper.Mapping(reader));
                }
            }

            return retDatas;
        }

        public Dictionary<string,double>Get月時数(List<DateData> dateData,int Year,int Month)
        {
            Dictionary<string, double> retDatas = new Dictionary<string, double>();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}", Year, Month.ToString("d2"));

            var joinDatas = dateData.Where(x => x.JIKANWARI.ToString("yyyyMM") == sb.ToString()).Join(MS_KYOUKA_CACHE.GetAll(), x => x.MS_KYOUKA_ID, j => j.MS_KYOUKA_ID, (x, j) => new
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

        public Dictionary<string, double> Get年時数(List<DateData> dateData)
        {
            Dictionary<string, double> retDatas = new Dictionary<string, double>();

            var joinDatas = dateData.Join(MS_KYOUKA_CACHE.GetAll(), x => x.MS_KYOUKA_ID, j => j.MS_KYOUKA_ID, (x, j) => new
            {
                KYOUKA_NAME = j.KYOUKA_NAME,
                RATIO = j.KYOUKA_RATIO
            });
            var groupDatas = joinDatas.GroupBy(x => x.KYOUKA_NAME).Select(a => new { KYOUKA_NAME = a.Key, Sum = a.Sum(x => x.RATIO) });

            foreach (var d in groupDatas)
            {
                retDatas.Add(d.KYOUKA_NAME, d.Sum);
            }
            return retDatas;
        }

        double ROUND = 10.0;

        public double 月合計(List<DateData> dateData, int Year, int Month)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}{1}", Year, Month.ToString("d2"));

            var joinDatas = dateData.Where(x => x.JIKANWARI.ToString("yyyyMM") == sb.ToString()).Join(MS_KYOUKA_CACHE.GetAll(), x => x.MS_KYOUKA_ID, j => j.MS_KYOUKA_ID, (x, j) => new
            {
                KYOUKA_NAME = j.KYOUKA_NAME,
                RATIO = j.KYOUKA_RATIO
            });

            return (int)(joinDatas.Sum(x => x.RATIO) * ROUND) / ROUND;
        }

        public void Save(List<DateData> dateDatas, MS_GAKUNEN Gakunen, int Year)
        {
            using (SQLiteTransaction trans = DBConnect.GetConnection().BeginTransaction())
            {
                DeleteYearMonth(Gakunen, Year);

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
        private void DeleteYearMonth(MS_GAKUNEN Gakunen, int Year)
        {
            #region SQL 
            string SQL = @"
delete from DATE_DATA

where MS_GAKUNEN_ID = :MS_GAKUNEN_ID
and strftime('%Y-%m', JIKANWARI) >= :NENDO_START and strftime('%Y-%m', JIKANWARI) <= :NENDO_END

";
            #endregion
            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                var 年度 = Get年度(Year);

                command.Parameters.AddWithValue(":MS_GAKUNEN_ID", Gakunen.MS_GAKUNEN_ID);
                command.Parameters.AddWithValue(":NENDO_START", 年度.NendoStart);
                command.Parameters.AddWithValue(":NENDO_END", 年度.NendoEnd);

                command.ExecuteNonQuery();
            }
        }
        (string NendoStart,string NendoEnd)Get年度(int Year)
        {
            (string NendoStart, string NendoEnd) ret;

            int end = Year + 1;

            ret.NendoStart = Year.ToString() + "-04";
            ret.NendoEnd = end.ToString() + "-03";

            return ret;
        }
    }
}
