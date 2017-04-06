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
KYOUKA_RATIO,
COLOR

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

        public List<(string KYOUKA_NAME, string COLOR)>Get教科一覧()
        {
            #region SQL
            string SQL = @"
select KYOUKA_NAME,COLOR

from MS_KYOUKA
group by KYOUKA_NAME
order by KYOUKA_NAME,COLOR

";
            #endregion

            var retDatas = new List<(string KYOUKA_NAME, string COLOR)>();

            using (SQLiteCommand command = new SQLiteCommand(SQL, DBConnect.GetConnection()))
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string k = reader.GetString(0);
                    string color;
                    if (reader.IsDBNull(1) == true)
                    {
                        color = "Black";
                    }
                    else
                    {
                        color = reader.GetString(1);
                    }
                    var data = (KYOUKA_NAME:k, COLOR:color);
                    retDatas.Add(data);
                }
            }

            return retDatas;

        }
    }
}
