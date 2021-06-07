using Infrastructure.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Views
{
	public class ApiKeyViewModel : BaseRecordViewModel
	{
		public string UserId { get; set; }
		public string Name { get; set; }
		public string Key { get; set; }

		public ICollection<string> Roles { get; set; }

	}
}
