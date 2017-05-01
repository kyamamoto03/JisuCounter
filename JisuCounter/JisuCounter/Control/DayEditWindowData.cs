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

        public DateTime Jikanwari;

        public List<DateData> DateDatas;
        public List<DateData> SaveDatas;


        public int MS_GAKUNEN_ID { get; set; }

        public void Init()
        {
            SaveDatas = new List<DateData>();
            if (DateDatas != null)
            {
                DateDatas.ForEach(x => SaveDatas.Add(x));
            }
            else
            {
                DateDatas = new List<DateData>();
            }
        }
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
            get
            {
                int SearchKoma = 1;
                return GetKoma(SearchKoma);
            }
            set
            {
                int SearchKoma = 1;
                SetKoma(value, SearchKoma);
                RaisePropertyChanged("Koma1");
            }
        }
        public MS_KYOUKA Koma2
        {
            get
            {
                int SearchKoma = 2;
                return GetKoma(SearchKoma);
            }
            set
            {
                int SearchKoma = 2;
                SetKoma(value, SearchKoma);
                RaisePropertyChanged("Koma2");
            }
        }

        public MS_KYOUKA Koma3
        {
            get
            {
                int SearchKoma = 3;
                return GetKoma(SearchKoma);
            }
            set
            {
                int SearchKoma = 3;
                SetKoma(value, SearchKoma);
                RaisePropertyChanged("Koma3");
            }
        }

        public MS_KYOUKA Koma4
        {
            get
            {
                int SearchKoma = 4;
                return GetKoma(SearchKoma);
            }
            set
            {
                int SearchKoma = 4;
                SetKoma(value, SearchKoma);
                RaisePropertyChanged("Koma4");
            }
        }

        public MS_KYOUKA Koma5
        {
            get
            {
                int SearchKoma = 5;
                return GetKoma(SearchKoma);
            }
            set
            {
                int SearchKoma = 5;
                SetKoma(value, SearchKoma);
                RaisePropertyChanged("Koma5");
            }
        }
        public MS_KYOUKA Koma6
        {
            get
            {
                int SearchKoma = 6;
                return GetKoma(SearchKoma);
            }
            set
            {
                int SearchKoma = 6;
                SetKoma(value, SearchKoma);
                RaisePropertyChanged("Koma6");
            }
        }
        public MS_KYOUKA Koma7
        {
            get
            {
                int SearchKoma = 7;
                return GetKoma(SearchKoma);
            }
            set
            {
                int SearchKoma = 7;
                SetKoma(value, SearchKoma);
                RaisePropertyChanged("Koma7");
            }
        }
        public MS_KYOUKA Koma8
        {
            get
            {
                int SearchKoma = 8;
                return GetKoma(SearchKoma);
            }
            set
            {
                int SearchKoma = 8;
                SetKoma(value, SearchKoma);
                RaisePropertyChanged("Koma8");
            }
        }

        private void SetKoma(MS_KYOUKA value, int SearchKoma)
        {
            var data = DateDatas.Where(x => x.KOMA == SearchKoma).FirstOrDefault();
            if (data == null)
            {
                data = new DateData();
                data.KOMA = SearchKoma;
                data.JIKANWARI = Jikanwari;
                data.MS_GAKUNEN_ID = MS_GAKUNEN_ID;
                DateDatas.Add(data);
            }
            data.MS_KYOUKA_ID = value.MS_KYOUKA_ID;
            
        }

        private MS_KYOUKA GetKoma(int SearchKoma)
        {
            var data = DateDatas.Where(x => x.KOMA == SearchKoma).FirstOrDefault();
            if (data == null)
                return null;

            return MS_KYOUKA_CACHE.Get(data.MS_KYOUKA_ID);
        }

        /// <summary>
        /// 追加した
        /// </summary>
        public void RollBack()
        {
            DateDatas.Clear();
            SaveDatas.ForEach(x => DateDatas.Add(x));
        }
    }
}
