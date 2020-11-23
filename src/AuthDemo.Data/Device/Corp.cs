using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class Corps : ProgramDB<Corps>
	{
		static Corps()
		{
			FirstPropertyIndex = 0;
			LastPropertyIndex = 1;
		}

		public long ID { get; set; }

		public string Name { get; set; }

		static public Corps GetCorpsByID(long CorpsID)
		{
			try
			{
				TryOpenConnection();

				CurrentQuery = "SELECT * FROM Corps " +
					"WHERE ID = @id;";

				AddParameter("@id", CorpsID);

				ExecuteReader();

				if (Reader.HasRows)
				{
					SwitchToNextRow();
					return GetEntityFromReader();
				}
				else
				{
					throw new NoSuchDataException(
						"Корпус не найден"
					);
				}
			}
			finally
			{
				FinishQuery();
			}
		}

		static public List<Corps> GetAll()
		{
			TryOpenConnection();

			var result = new List<Corps>();

			try
			{
				CurrentQuery =
					$"SELECT * FROM [Corps];";
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

		private static Corps GetEntityFromReader()
		{
			var result = new Corps()
			{
				ID = (long)ReadColumnFromRow(0),
				Name = (string)ReadColumnFromRow(1)
			};

			return result;
		}

		public override string ToString() => Name;
	}
}
