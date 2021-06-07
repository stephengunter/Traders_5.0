using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Indicators
{
    public interface IIndicator
    {
        int Begin { get; set; } //盤中產生信號開始時間  例如 90000

        int End { get; set; } //盤中產生信號結束時間  例如 133000

        bool Main { get; set; }  //主圖

        IndicatorType Type { get; set; }
        
        void Calculate();
    }

    public enum IndicatorType
    {
        None,
        Curve,
        Bar
    }
    public abstract class BaseIndicator : IIndicator
    {
        public int Begin { get; set; } //盤中產生信號開始時間  例如 90000

        public int End { get; set; } //盤中產生信號結束時間  例如 133000

        public bool Main { get; set; }

        public IndicatorType Type { get; set; }

        public abstract void Calculate();
    }
}
