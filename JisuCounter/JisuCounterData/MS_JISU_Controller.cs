using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace JisuCounterData
{
    public class MS_JISU_Controller
    {
        public List<MS_JISU> GetAt(MS_GAKUNEN MsGakune)
        {
            #region SQL 
            string SQL = @"
select
MS_GAKUNEN_ID,
KYOUKA_NAME,
JISU

from MS_JISU
where MS_GAKUNEN_ID = :MS_GAKUNEN_ID
";
            #endregion
            List<MS_JISU> retDatas = new List<MS_JISU>();

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                command.Parameters.AddWithValue(":MS_GAKUNEN_ID", MsGakune.MS_GAKUNEN_ID);

                var reader = command.ExecuteReader();
                var mapper = new Mapper<MS_JISU>();
                while (reader.Read())
                {
                    retDatas.Add(mapper.Mapping(reader));
                }
            }

            return retDatas;
        }

        /// <summary>
        /// 学年の時数マスタを更新（削除の後挿入）
        /// </summary>
        /// <param name="MsGakune"></param>
        /// <param name="Jisus"></param>
        public void Inserts(MS_GAKUNEN MsGakune, List<MS_JISU> Jisus)
        {

            using (SQLiteTransaction trans = DBConnect.GetConnection().BeginTransaction())
            {
                ///削除
                Deletes(MsGakune);

                ///挿入
                Jisus.ForEach(x =>
                {
                    Insert(x);
                });
                trans.Commit();
            }
        }

        private void Insert(MS_JISU Jisu)
        {
            #region SQL
            string SQL = @"
insert into MS_JISU(
MS_GAKUNEN_ID,
KYOUKA_NAME,
JISU)
values(
:MS_GAKUNEN_ID,
:KYOUKA_NAME,
:JISU)
";
            #endregion

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                command.Parameters.AddWithValue(":MS_GAKUNEN_ID", Jisu.MS_GAKUNEN_ID);
                command.Parameters.AddWithValue(":KYOUKA_NAME", Jisu.KYOUKA_NAME);
                command.Parameters.AddWithValue(":JISU", Jisu.JISU);

                command.ExecuteNonQuery();
            }
        }

        private void Deletes(MS_GAKUNEN MsGakune)
        {
            #region SQL 
            string SQL = @"
delete from MS_JISU
where MS_GAKUNEN_ID = :MS_GAKUNEN_ID
";
            #endregion

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                command.Parameters.AddWithValue(":MS_GAKUNEN_ID", MsGakune.MS_GAKUNEN_ID);

                command.ExecuteNonQuery();
            }
        }
    }
}
