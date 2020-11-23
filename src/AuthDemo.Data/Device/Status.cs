using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class Status : ProgramDB<Status>
	{
		public Status()
		{
			FirstPropertyIndex = 0;
			LastPropertyIndex = 1;
		}

		public long ID { get; set; }

		public string Name { get; set; }

		static public Status GetStatusByID(long statusID)
		{
			try
			{
				TryOpenConnection();

				CurrentQuery = "SELECT * FROM [Status] WHERE ID = @statusID;";

				AddParameter("@statusID", statusID);

				ExecuteReader();

				if (Reader.HasRows)
				{
					SwitchToNextRow();
					return GetEntityFromReader();
				}
				else throw new NoSuchDataException(
					"Нет статуса с таким ID"
				);
			}
			finally { FinishQuery(); }
		}

		static public Status GetStatusByName(string statusName)
		{
			try
			{
				TryOpenConnection();

				CurrentQuery = "SELECT * FROM [Status] WHERE Name = @statusName;";

				AddParameter("@statusName", statusName);

				ExecuteReader();

				if (Reader.HasRows)
				{
					SwitchToNextRow();
					return GetEntityFromReader();
				}
				else throw new NoSuchDataException(
					"Нет статуса с таким именем"
				);
			}
			finally { FinishQuery(); }
		}

		static public List<Status> GetAll()
		{
			TryOpenConnection();

			var result = new List<Status>();

			try
			{
				CurrentQuery =
					$"SELECT * FROM [Status];";
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

		static Status GetEntityFromReader()
		{
			var result = new Status();

			result.ID = (long)ReadColumnFromRow(0);
			result.Name = (string)ReadColumnFromRow(1);

			return result;
		}

		public override string ToString() => Name;
	}
}
