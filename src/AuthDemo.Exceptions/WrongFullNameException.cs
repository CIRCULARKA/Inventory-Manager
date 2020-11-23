using System;

namespace AuthDemo.Exceptions
{
	internal class WrongFullNameException : Exception
	{
		public WrongFullNameException(
			string msg = "ФИО введено неверно"
		) : base(msg) { }
	}
}
