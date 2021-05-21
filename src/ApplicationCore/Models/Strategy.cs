using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using ApplicationCore.Helpers;
using ApplicationCore.Indicators;
using Infrastructure.Entities;

namespace ApplicationCore.Models
{
	public class Strategy : BaseEntity
	{
		public string UserId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public ICollection<IndicatorSettings> IndicatorSettings { get; set; } = new List<IndicatorSettings>();

		public bool Default { get; set; }

        public decimal Cost { get; set; } //交易成本

        public int STPL { get; set; } //停損

		public int STPW { get; set; } //停利

		public int OutTime { get; set; } //收盤出場時間

		public bool NoStop => STPL == 0 && STPW == 0;



        List<IIndicator> indicators = new List<IIndicator>();

        public void Init(List<Indicator> indicators, List<Quote> quotes)
        {
            for (var i = 0; i < indicators.Count; i++)
            {
                Indicator indicator = null;
                if (indicators[i].Entity == nameof(BlueChips))
                {
                    indicator = indicators[i] as BlueChips;
                }

                var settings = IndicatorSettings.Where(item => item.IndicatorId == indicators[i].Id).FirstOrDefault();
                indicator.Init(quotes, settings);

                this.indicators.Add(indicator);
            }
        }


    }


	public class IndicatorSettings : BaseEntity
	{
		public int StrategyId { get; set; }

		public int IndicatorId { get; set; }

		public int Arg { get; set; }

		public int Order { get; set; }
	}
}
