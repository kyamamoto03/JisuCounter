using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using JisuCounterData;
using System.Drawing;

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

        string DEFALUT_COLOR = "Transparent";

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
            RaisePropertyChanged("Koma1_Foreground");
            RaisePropertyChanged("Koma2_Foreground");
            RaisePropertyChanged("Koma3_Foreground");
            RaisePropertyChanged("Koma4_Foreground");
            RaisePropertyChanged("Koma5_Foreground");
            RaisePropertyChanged("Koma6_Foreground");
            RaisePropertyChanged("Koma7_Foreground");
            RaisePropertyChanged("Koma8_Foreground");
            RaisePropertyChanged("KOMA1");
            RaisePropertyChanged("KOMA2");
            RaisePropertyChanged("KOMA3");
            RaisePropertyChanged("KOMA4");
            RaisePropertyChanged("KOMA5");
            RaisePropertyChanged("KOMA6");
            RaisePropertyChanged("KOMA7");
            RaisePropertyChanged("KOMA8");
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
            RaisePropertyChanged("KOMA7");
            RaisePropertyChanged("KOMA8");
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

        public string Koma1_Foreground
        {
            get
            {
                int KOMA = 1;
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();
                
                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    if (kyouka.COLOR != null)
                    {
                        return kyouka.COLOR; 
                    }
                }
                return DEFALUT_COLOR;
            }
        }
        public string Koma2_Foreground
        {
            get
            {
                int KOMA = 2;
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    if (kyouka.COLOR != null)
                    {
                        return kyouka.COLOR;
                    }
                }
                return DEFALUT_COLOR;
            }
        }
        public string Koma3_Foreground
        {
            get
            {
                int KOMA = 3;
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    if (kyouka.COLOR != null)
                    {
                        return kyouka.COLOR;
                    }
                }
                return DEFALUT_COLOR;
            }
        }
        public string Koma4_Foreground
        {
            get
            {
                int KOMA = 4;
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    if (kyouka.COLOR != null)
                    {
                        return kyouka.COLOR;
                    }
                }
                return DEFALUT_COLOR;
            }
        }
        public string Koma5_Foreground
        {
            get
            {
                int KOMA = 5;
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    if (kyouka.COLOR != null)
                    {
                        return kyouka.COLOR;
                    }
                }
                return DEFALUT_COLOR;
            }
        }
        public string Koma6_Foreground
        {
            get
            {
                int KOMA = 6;
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    if (kyouka.COLOR != null)
                    {
                        return kyouka.COLOR;
                    }
                }
                return DEFALUT_COLOR;
            }
        }
        public string Koma7_Foreground
        {
            get
            {
                int KOMA = 7;
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    if (kyouka.COLOR != null)
                    {
                        return kyouka.COLOR;
                    }
                }
                return DEFALUT_COLOR;
            }
        }
        public string Koma8_Foreground
        {
            get
            {
                int KOMA = 8;
                var KomaData = _DateDates.Where(x => x.KOMA == KOMA).FirstOrDefault();

                if (KomaData != null)
                {
                    var kyouka = MS_KYOUKA_CACHE.Get(KomaData.MS_KYOUKA_ID);
                    if (kyouka.COLOR != null)
                    {
                        return kyouka.COLOR;
                    }
                }
                return DEFALUT_COLOR;
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
        public string KOMA7
        {
            get
            {
                int KOMA = 7;
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
        public string KOMA8
        {
            get
            {
                int KOMA = 8;
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
