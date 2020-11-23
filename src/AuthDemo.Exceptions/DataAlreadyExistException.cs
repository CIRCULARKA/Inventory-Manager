using System;

namespace AuthDemo.Exceptions
{
	class DataAlreadyExistException : Exception
	{
		public DataAlreadyExistException(string msg) : base(msg) { }
	}
}
