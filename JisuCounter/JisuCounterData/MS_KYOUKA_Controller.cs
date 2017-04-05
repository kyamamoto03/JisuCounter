using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace JisuCounterData
{
    public class MS_KYOUKA_Controller
    {
        public IList<MS_KYOUKA>GetAll()
        {
            #region SQL
            string SQL = @"
select 
MS_KYOUKA_ID,
KYOUKA_NAME,
KYOUKA_RATIO
from MS_KYOUKA
order by MS_KYOUKA_ID

";
            #endregion

            List<MS_KYOUKA> retDatas = new List<MS_KYOUKA>();

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                var reader = command.ExecuteReader();
                var mapper = new Mapper<MS_KYOUKA>();
                while(reader.Read())
                {
                    retDatas.Add(mapper.Mapping(reader));
                }
            }

            return retDatas;
        }

        public List<string>Get教科一覧()
        {
            #region SQL
            string SQL = @"
select KYOUKA_NAME

from MS_KYOUKA
group by KYOUKA_NAME
order by KYOUKA_NAME

";
            #endregion

            List<string> retDatas = new List<string>();

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    retDatas.Add(reader.GetString(0));
                }
            }

            return retDatas;

        }
    }
}
