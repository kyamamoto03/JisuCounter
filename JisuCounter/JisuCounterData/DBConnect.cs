using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace JisuCounterData
{
    public class DBConnect : IDisposable
    {
        static DBConnect _instance = new DBConnect();

        SQLiteConnection _conn;

        public static DBConnect GetInstance()
        {
            return _instance;
        }

        public static SQLiteConnection GetConnection()
        {
            return _instance._conn;
        }

        public void Open(string connectString)
        {
            _conn = new SQLiteConnection(connectString);
            _conn.Open();
        }

        public void Dispose()
        {
            _conn?.Dispose();
        }
    }
}
