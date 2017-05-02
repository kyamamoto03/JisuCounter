using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using JisuCounterData;

using System.Windows.Media;

namespace JisuCounter.JisuMaster
{
    class JisuMasterWindowData
    {
        /// <summary>
        /// 合計字数の幅
        /// </summary>
        double JISU_WIDTH = 40d;

        Color FORE_COLOR = Colors.White;

        MS_GAKUNEN _TargetGakunenID;
        public MS_GAKUNEN TargetGakunenID {
            get { return _TargetGakunenID; }
            set
            {
                MS_JISU_Controller MsJisuController = new MS_JISU_Controller();

                _TargetGakunenID = value;    
                m_Jisus = MsJisuController.GetAt(_TargetGakunenID);
            }
        }

        internal List<MS_JISU> m_Jisus = new List<MS_JISU>();
        TextBox SumTextBox;

        public void Load(Panel YearSum)
        {
            MakeYearSumBase(YearSum);
            MakeYearSum();
        }

        #region 年合計
        class YearSumLabel
        {
            internal Label label;
            /// <summary>
            /// 年で決められている時数
            /// </summary>
            internal TextBox JisuLabel;
        }
        List<YearSumLabel> YearSumLabels = new List<YearSumLabel>();

        public void MakeYearSumBase(Panel YearSum)
        {
            MS_KYOUKA_Controller MsKyoukaController = new MS_KYOUKA_Controller();


            var Kyoukas = MsKyoukaController.Get教科一覧();

            foreach (var kyouka in Kyoukas)
            {
                var l = MakeYearSumLabel(kyouka);
                YearSumLabels.Add(l.SumLabels);
                YearSum.Children.Add(l.Grid);
            }

            ///合計を追加
            var a = MakeYearSumLabel(("合計", "Black"));
            YearSumLabels.Add(a.SumLabels);
            YearSum.Children.Add(a.Grid);

            SumTextBox = a.SumLabels.JisuLabel;
            SumTextBox.IsReadOnly = true;
        }

        (YearSumLabel SumLabels, Grid Grid) MakeYearSumLabel((string KYOUKA_NAME, string COLOR) labelData)
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            Label label = new Label();
            label.SetValue(Grid.ColumnProperty, 0);
            label.Content = labelData.KYOUKA_NAME;

            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(labelData.COLOR);

            label.Foreground = new SolidColorBrush(FORE_COLOR);
            label.Background = new SolidColorBrush(color);
            grid.Children.Add(label);

            TextBox JisuLabel = new TextBox();
            JisuLabel.SetValue(Grid.ColumnProperty, 1);
            JisuLabel.Text = "0";
            JisuLabel.Width = JISU_WIDTH;
            grid.Children.Add(JisuLabel);


            YearSumLabel sumLabels = new YearSumLabel { label = label, JisuLabel = JisuLabel };
            return (sumLabels, grid);


        }

        /// <summary>
        /// 次ごとの教科別時数合計(年)
        /// </summary>
        public void MakeYearSum()
        {
            DateDataController DateDataController = new DateDataController();

            //一度合計値を０にする
            foreach (var control in YearSumLabels)
            {
                var jisu = m_Jisus.Where(x => x.KYOUKA_NAME == (string)control.label.Content).FirstOrDefault();
                if (jisu != null)
                {
                    control.JisuLabel.Text = jisu.JISU.ToString();
                    control.JisuLabel.TextChanged += JisuLabel_TextChanged;
                }
            }

            JisuSum();
        }

        private void JisuLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            JisuSum();
        }

        private void JisuSum()
        {
            double SumJisu = 0;
            foreach (var control in YearSumLabels)
            {
                var jisu = m_Jisus.Where(x => x.KYOUKA_NAME == (string)control.label.Content).FirstOrDefault();
                if (jisu != null)
                {
                    double j;
                    if (double.TryParse(control.JisuLabel.Text, out j))
                    {
                        SumJisu += j;
                    }
                }
            }
            SumTextBox.Text = SumJisu.ToString();
        }

        /// <summary>
        /// 字数を取り込む
        /// </summary>
        public void SetYearData()
        {
            foreach (var control in YearSumLabels)
            {
                var jisu = m_Jisus.Where(x => x.KYOUKA_NAME == (string)control.label.Content).FirstOrDefault();
                if (jisu != null)
                {
                    double j;
                    if (double.TryParse(control.JisuLabel.Text,out j))
                    {
                        jisu.JISU = j;
                    }
                }
            }
        }
        #endregion
    }
}
