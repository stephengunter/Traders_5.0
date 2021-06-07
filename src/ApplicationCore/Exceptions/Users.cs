using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Exceptions
{
	public class CreateUserException : Exception
	{
		public CreateUserException(string message) : base(message)
		{

		}
	}

	public class UserNotFoundException : Exception
	{
		public UserNotFoundException(string val, string key = "Id") : base($"UserNotFound. {key}: {val}")
		{

		}
	}

	

	public class UserPasswordException : Exception
	{
		public UserPasswordException(string message) : base(message)
		{

		}
	}

	public class WrongPasswordException : Exception
	{
		public WrongPasswordException(string message) : base(message)
		{

		}
	}
}
