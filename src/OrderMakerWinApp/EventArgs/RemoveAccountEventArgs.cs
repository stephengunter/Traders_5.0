using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderMakerWinApp
{
	public class RemoveAccountEventArgs : EventArgs
	{
		public RemoveAccountEventArgs(string id)
		{
			this.Id = id;
		}

		public string Id { get; private set; }
	}
}
