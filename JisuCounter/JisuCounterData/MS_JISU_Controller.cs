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
        public List<MS_JISU>GetAt(MS_GAKUNEN MsGakune)
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
    }
}
