using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using ApplicationCore.Helpers;
using Infrastructure.Entities;

namespace ApplicationCore.Models
{
    public interface IIndicator
    {
        string Name { get; set; }

        string Description { get; set; }

        int Begin { get; set; } //盤中產生信號開始時間  例如 90000

        int End { get; set; } //盤中產生信號結束時間  例如 133000

        string Entity { get; set; }

        string Params { get; set; }

        bool Main { get; set; }

        IndicatorType Type { get; set; }

        SourceType Source { get; set; }

        bool WithAvg { get; set; }

        int CoverId { get; set; }



        void Init(List<Quote> quotes, IndicatorSettings settings);
        void Calculate();
    }

    public enum IndicatorType
	{
		None,
		Curve,
		Bar
	}

	public enum SourceType
	{
		Futures,
		Stock,
		Complex,
		Market
	}

	public class Indicator : BaseRecord, IIndicator
    {
		public string Name { get; set; }

		public string Description { get; set; }

		public int Begin { get; set; } //盤中產生信號開始時間  例如 90000

		public int End { get; set; } //盤中產生信號結束時間  例如 133000

		public string Entity { get; set; }

		public string Params { get; set; }

		public bool Main { get; set; }

		public IndicatorType Type { get; set; }

		public SourceType Source { get; set; }

		public bool WithAvg { get; set; }

		public int CoverId { get; set; }


		public string[] ResolvedParams => Params.Split(',');

		public int DefaultParam => Params.Split(',')[2].ToInt();

		public IEnumerable<int> ResolveParamsValues()
		{
			int min = ResolvedParams[0].ToInt();
			int max = ResolvedParams[1].ToInt();

			var result = new List<int>();
			for (int i = min; i <= max; i++) result.Add(i);
			return result;
		}



        private List<Quote> quotes;
        private IndicatorSettings settings;

        public virtual void Init(List<Quote> quotes, IndicatorSettings settings)
        {
            this.quotes = quotes;
            this.settings = settings;
        }

        public virtual void Calculate()
        {
            

        }

       

        bool IsDataInTime(Data data) => data.Time > this.Begin && data.Time <= this.End;

    }
	
}
