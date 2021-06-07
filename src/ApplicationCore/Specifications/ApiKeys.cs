using ApplicationCore.Models;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Helpers;

namespace ApplicationCore.Specifications
{
    public class ApiKeyFilterSpecification : BaseSpecification<ApiKey>
    {
		public ApiKeyFilterSpecification(string key) : base(x => x.Key == key)
		{
			AddInclude(x => x.User);
		}
		public ApiKeyFilterSpecification(User user) : base(x => x.UserId == user.Id)
		{
			
		}
	}
}
