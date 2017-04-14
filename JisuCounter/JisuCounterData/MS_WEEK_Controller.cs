using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace JisuCounterData
{
    public class MS_WEEK_Controller
    {
        public List<MS_WEEK>Get()
        {
            #region SQL 
            string SQL = @"
select DAY,KOMA,MS_KYOUKA_ID from MS_WEEK
order by DAY,KOMA
";
            #endregion
            List<MS_WEEK> retDatas = new List<MS_WEEK>();

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                var reader = command.ExecuteReader();
                var mapper = new Mapper<MS_WEEK>();
                while (reader.Read())
                {
                    retDatas.Add(mapper.Mapping(reader));
                }
            }

            return retDatas;
        }

        public void Updates(List<MS_WEEK> MsWeeks)
        {
            using (SQLiteTransaction trans = DBConnect.GetConnection().BeginTransaction())
            {
                try
                {
                    DeleteAll();
                    foreach (var week in MsWeeks)
                    {
                        Insert(week);
                    }
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }
        private void Insert(MS_WEEK MsWeek)
        {
            #region SQL 
            string SQL = @"
insert into MS_WEEK(
DAY,
KOMA,
MS_KYOUKA_ID)
values(
:DAY,
:KOMA,
:MS_KYOUKA_ID)

";
            #endregion
            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                command.Parameters.AddWithValue(":DAY", MsWeek.DAY);
                command.Parameters.AddWithValue(":KOMA", MsWeek.KOMA);
                command.Parameters.AddWithValue(":MS_KYOUKA_ID", MsWeek.MS_KYOUKA_ID);

                command.ExecuteNonQuery();
            }

        }
        private void DeleteAll()
        {
            #region SQL 
            string SQL = @"
delete from MS_WEEK

";
            #endregion
            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
