using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Exceptions
{
	public class UserNoRoleException : Exception
	{
		public UserNoRoleException(string message) : base(message)
		{

		}
	}

	public class AddUserToRoleException : Exception
	{
		public AddUserToRoleException(string message) : base(message)
		{

		}
	}

	public class RemoveUserToRoleException : Exception
	{
		public RemoveUserToRoleException(string message) : base(message)
		{

		}
	}

}
