using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class IPAddress : ProgramDB<IPAddress>
	{
		static IPAddress()
		{
			FirstPropertyIndex = 0;
			LastPropertyIndex = 0;
		}

		public long ID { get; set; }
		public string Address { get; set; }

		static public IPAddress GetIPByID(long id)
		{
			try
			{
				TryOpenConnection();

				CurrentQuery = "SELECT * FROM IPAddress WHERE ID = @id;";

				AddParameter("@id", id);

				ExecuteReader();

				if (Reader.HasRows)
				{
					SwitchToNextRow();
					return GetEntityFromReader();
				}
				else throw new NoSuchDataException("Нет IP-адреса с таким ID");
			}
			finally { FinishQuery(); }
		}

		static public IPAddress GetIPByAddress(string address)
		{
			try
			{
				TryOpenConnection();

				CurrentQuery = "SELECT * FROM IPAddress WHERE Address = @address;";

				AddParameter("@address", address);

				ExecuteReader();

				if (Reader.HasRows)
				{
					SwitchToNextRow();
					return GetEntityFromReader();
				}
				else throw new NoSuchDataException("Такого адреса нет");
			}
			finally { FinishQuery(); }
		}

		static public List<IPAddress> GetAll()
		{
			TryOpenConnection();

			var result = new List<IPAddress>();

			try
			{
				CurrentQuery =
					$"SELECT * FROM [IPAddress];";
				ExecuteReader();

				if (Reader.HasRows)
					for (; SwitchToNextRow();)
						result.Add(GetEntityFromReader());
				else
					throw new NoSuchDataException();
			}
			finally { FinishQuery(); }

			return result;
		}

		static IPAddress GetEntityFromReader()
		{
			var result = new IPAddress();

			result.ID = (long)ReadColumnFromRow(0);
			result.Address = (string)ReadColumnFromRow(1);

			return result;
		}

		public override string ToString() => this.Address;
	}
}
