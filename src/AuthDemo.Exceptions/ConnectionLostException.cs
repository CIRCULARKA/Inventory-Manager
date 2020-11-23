using System;

namespace AuthDemo.Exceptions
{
	internal class ConnectionLostException : Exception
	{
		public ConnectionLostException(
			string msg = "Не удалось установить соединение с базой данных"
		) : base(msg) { }
	}
}
