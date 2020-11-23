using System;

namespace AuthDemo.Exceptions
{
	internal class NoSuchDataException : Exception
	{
		public NoSuchDataException(
			string msg = "Данных нет"
		) : base(msg) { }
	}
}
