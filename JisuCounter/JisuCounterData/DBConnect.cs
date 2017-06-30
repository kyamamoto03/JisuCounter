using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JisuCounterData
{
    public class DBConnect : IDisposable
    {
        static DBConnect _instance = new DBConnect();

        MySqlConnection _conn;

        public static DBConnect GetInstance()
        {
            return _instance;
        }

        public static MySqlConnection GetConnection()
        {
            return _instance._conn;
        }

        public void Open(string connectString)
        {
            _conn = new MySqlConnection(connectString);
            _conn.Open();
        }

        public void Dispose()
        {
            _conn?.Dispose();
        }
    }
}
