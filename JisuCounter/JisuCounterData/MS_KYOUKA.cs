using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JisuCounterData
{
    public class MS_KYOUKA
    {
        public override string ToString()
        {
            if (MS_KYOUKA_ID > 0)
            {
                return KYOUKA_NAME + " " + KYOUKA_RATIO;
            }else
            {
                return KYOUKA_NAME;
            }
        }

        public int MS_KYOUKA_ID { get; set; }
        public string KYOUKA_NAME { get; set; }
        public float KYOUKA_RATIO { get; set; }
        public string COLOR { get; set; }

    }

    public class MS_KYOUKA_CACHE
    {
        static MS_KYOUKA_CACHE _instance = new MS_KYOUKA_CACHE();
        IList<MS_KYOUKA> CacheData = new List<MS_KYOUKA>();

        public static MS_KYOUKA_CACHE GetInstance()
        {
            return _instance;
        }
        public static void CacheFill()
        {
            MS_KYOUKA_Controller MsKyoukaController = new MS_KYOUKA_Controller();
            _instance.CacheData = MsKyoukaController.GetAll();
            _instance.CacheData.Insert(0, new MS_KYOUKA { KYOUKA_NAME = "", MS_KYOUKA_ID = -1 });
        }
        public static MS_KYOUKA Get(int MS_KYOUKA_ID)
        {
            return _instance.CacheData.Where(x => x.MS_KYOUKA_ID == MS_KYOUKA_ID).FirstOrDefault();
        }
        public static IList<MS_KYOUKA> GetAll()
        {
            return _instance.CacheData;
        }
    }
}
