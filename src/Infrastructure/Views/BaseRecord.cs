using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace Infrastructure.Views
{
	public abstract class BaseRecordViewModel : BaseEntityViewModel
	{
		public DateTime CreatedAt { get; set; }
		public DateTime LastUpdated { get; set; }
		public string UpdatedBy { get; set; }

		public int Order { get; set; }
		public bool Removed { get; set; }

		public bool Active { get; set; }

		public virtual string StatusText => this.Active ? "上架中" : "已下架";

		public string CreatedAtText => CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");

		public string LastUpdatedText => LastUpdated.ToString("yyyy-MM-dd HH:mm:ss");

		public virtual bool CanRemove
		{
			get
			{
				if (Id > 0)
				{
					return !Active;
				}
				return false;
			}
	    }

		public void SetUpdated(string userId)
		{
			UpdatedBy = userId;
			LastUpdated = DateTime.Now;
		}

	}
	
}
