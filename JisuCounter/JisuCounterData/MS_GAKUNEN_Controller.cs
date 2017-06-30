using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JisuCounterData
{
    public class MS_GAKUNEN_Controller
    {
        public IList<MS_GAKUNEN>GetAll()
        {
            #region SQL
            string SQL = @"
select 
MS_GAKUNEN_ID,
GAKUNEN_NAME,
SHOW_ORDER
from MS_GAKUNEN
order by SHOW_ORDER

 ";
            #endregion

            List<MS_GAKUNEN> retDatas = new List<MS_GAKUNEN>();

            using (MySqlCommand command = new MySqlCommand(SQL, DBConnect.GetConnection()))
            {
                using (var reader = command.ExecuteReader())
                {
                    var mapper = new Mapper<MS_GAKUNEN>();
                    while (reader.Read())
                    {
                        retDatas.Add(mapper.Mapping(reader));
                    }
                }
            }

            return retDatas;
        }
    }
}
