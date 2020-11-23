using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class Cabinet : ProgramDB<Cabinet>
	{
		static Cabinet()
		{
			FirstPropertyIndex = 0;
			LastPropertyIndex = 2;
		}

		public long ID { get; set; }

		public long CorpsID { get; set; }

		public string Name { get; set; }

		static public Cabinet GetCabinetByID(long id)
		{
			TryOpenConnection();

			CurrentQuery =
				$"SELECT * FROM [Cabinet] WHERE ID = @id;";

			AddParameter("@id", id);
			ExecuteReader();

			if (Reader.HasRows)
				SwitchToNextRow();
			else throw new NoSuchDataException("Кабинета с таким ID нет");

			var result = new Cabinet();
			result.ID = (long)ReadColumnFromRow(0);
			result.CorpsID = (long)ReadColumnFromRow(1);
			result.Name = (string)ReadColumnFromRow(2);

			FinishQuery();

			return result;
		}

		static public List<Cabinet> GetAll(Corps Corps)
		{
			TryOpenConnection();

			var result = new List<Cabinet>();

			try
			{
				CurrentQuery =
					$"SELECT * FROM [Cabinet] " +
						"WHERE CorpsID = @CorpsID;";

				AddParameter("@CorpsID", Corps.ID);

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

		static Cabinet GetEntityFromReader()
		{
			var result = new Cabinet();

			result.ID = (long)ReadColumnFromRow(0);
			result.CorpsID = (long)ReadColumnFromRow(1);
			result.Name = (string)ReadColumnFromRow(2);

			return result;
		}

		public override string ToString() => Name;
	}
}
