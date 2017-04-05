using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using JisuCounterData;

namespace JisuCounter.Control
{
    public class DayControlData : INotifyPropertyChanged
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

        public DayControlData()
        {
            _DateDates = new List<DateData>();
        }


        List<DateData> _DateDates { get; set; }
        public List<DateData> DateDatas
        {
            get { return _DateDates; }
            set
            {
                _DateDates = value;
            }
        }

        public void Refresh()
        {
            RaisePropertyChanged("KOMA1");
            RaisePropertyChanged("KOMA2");
            RaisePropertyChanged("KOMA3");
            RaisePropertyChanged("KOMA4");
            RaisePropertyChanged("KOMA5");
            RaisePropertyChanged("KOMA6");
        }
        public void Add(IEnumerable<DateData> dateDatas)
        {
            if (dateDatas != null)
            {
                foreach (var d in dateDatas)
                {
                    _DateDates.Add(d);
                }
            }
            RaisePropertyChanged("KOMA1");
            RaisePropertyChanged("KOMA2");
            RaisePropertyChanged("KOMA3");
            RaisePropertyChanged("KOMA4");
            RaisePropertyChanged("KOMA5");
            RaisePropertyChanged("KOMA6");
        }

        DateTime _Day;
        public DateTime Day
        {
            get { return _Day; }
            set
            {
                _Day = value;
                RaisePropertyChanged("Day");
            }
        }

        public string KOMA1
        {
            get
            {
                int KOMA = 1;
                StringBuilder sb = new StringBuilder();
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    sb.AppendFormat("{0} {1}", kyouka.KYOUKA_NAME, kyouka.KYOUKA_RATIO);
                }
                return sb.ToString();
            }
        }
        public string KOMA2
        {
            get
            {
                int KOMA = 2;
                StringBuilder sb = new StringBuilder();
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    sb.AppendFormat("{0} {1}", kyouka.KYOUKA_NAME, kyouka.KYOUKA_RATIO);
                }
                return sb.ToString();
            }
        }
        public string KOMA3
        {
            get
            {
                int KOMA = 3;
                StringBuilder sb = new StringBuilder();
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    sb.AppendFormat("{0} {1}", kyouka.KYOUKA_NAME, kyouka.KYOUKA_RATIO);
                }
                return sb.ToString();
            }
        }
        public string KOMA4
        {
            get
            {
                int KOMA = 4;
                StringBuilder sb = new StringBuilder();
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    sb.AppendFormat("{0} {1}", kyouka.KYOUKA_NAME, kyouka.KYOUKA_RATIO);
                }
                return sb.ToString();
            }
        }
        public string KOMA5
        {
            get
            {
                int KOMA = 5;
                StringBuilder sb = new StringBuilder();
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    sb.AppendFormat("{0} {1}", kyouka.KYOUKA_NAME, kyouka.KYOUKA_RATIO);
                }
                return sb.ToString();
            }
        }
        public string KOMA6
        {
            get
            {
                int KOMA = 6;
                StringBuilder sb = new StringBuilder();
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    sb.AppendFormat("{0} {1}", kyouka.KYOUKA_NAME, kyouka.KYOUKA_RATIO);
                }
                return sb.ToString();
            }
        }
    }
}
