using System;

namespace AuthDemo.Exceptions
{
	internal class WrongPasswordException : Exception
	{
		public WrongPasswordException(
			string msg = "Пароль введёно неверно"
		) : base(msg) { }
	}
}
