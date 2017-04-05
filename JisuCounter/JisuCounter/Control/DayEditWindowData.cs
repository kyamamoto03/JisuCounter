using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using JisuCounterData;

namespace JisuCounter.Control
{
    public class DayEditWindowData : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string PropertyName)
        {
            var d = PropertyChanged;
            if (d != null)
            {
                d(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion

        public List<DateData> DateDatas;

        ObservableCollection<MS_KYOUKA> _KyoukaMaster = new ObservableCollection<MS_KYOUKA>();
        public ObservableCollection<MS_KYOUKA>KyoukaMaster
        {
            get { return _KyoukaMaster; }
            set
            {
                _KyoukaMaster = value;
                RaisePropertyChanged("KyoukaMaster");
            }
        }

        public void LoadKyoukaMaster()
        {
            var Kyoukas = MS_KYOUKA_CACHE.GetAll();
            KyoukaMaster.Clear();
            foreach(var Kyouka in Kyoukas)
            {
                KyoukaMaster.Add(Kyouka);
            }

        }

        public MS_KYOUKA Koma1
        {
            get {
                int SearchKoma = 1;
                var data = DateDatas.Where(x => x.KOMA == SearchKoma).FirstOrDefault();
                if (data == null)
                    return null;

                return MS_KYOUKA_CACHE.Get(data.MS_KYOUKA_ID);
            }
            set
            {
                int SearchKoma = 1;
                var data = DateDatas.Where(x => x.KOMA == SearchKoma).FirstOrDefault();
                if (data == null)
                {
                    DateData insertData = new DateData();

                }
                else
                {
                    data.MS_KYOUKA_ID = value.MS_KYOUKA_ID;
                }
                RaisePropertyChanged("Koma1");
            }
        }
    }
}
