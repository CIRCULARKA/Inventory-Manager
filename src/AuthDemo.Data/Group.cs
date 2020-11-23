using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class Group : ProgramDB<Group>
	{
		public Group()
		{
			FirstPropertyIndex = 1;
			LastPropertyIndex = 1;
		}

		public long ID { get; set; }

		public string Name { get; set; }

		/// <summary>
		/// Returns -1 if there is no such a group
		/// </summary>
		static public Group GetGroupByName(string groupName)
		{
			TryOpenConnection();

			var result = new Group();

			CurrentQuery =
				$"SELECT * FROM [Group] WHERE Name = @groupName;";

			AddParameter("@groupName", groupName);

			ExecuteReader();
			SwitchToNextRow();

			result.ID = (long)ReadColumnFromRow(0);
			result.Name = (string)ReadColumnFromRow(1);

			FinishQuery();

			return result;
		}

		static public Group GetGroupByID(long id)
		{
			TryOpenConnection();

			CurrentQuery =
				$"SELECT * FROM [Group] WHERE ID = @id;";

			AddParameter("@id", id);
			ExecuteReader();

			SwitchToNextRow();

			var result = new Group();
			result.ID = (long)ReadColumnFromRow(0);
			result.Name = (string)ReadColumnFromRow(1);

			FinishQuery();

			return result;
		}

		static public List<Group> GetAll()
		{
			TryOpenConnection();

			var result = new List<Group>();

			try
			{
				CurrentQuery =
					$"SELECT * FROM [Group];";
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

		static Group GetEntityFromReader()
		{
			var result = new Group();

			result.ID = (long)ReadColumnFromRow(0);
			result.Name = (string)ReadColumnFromRow(1);

			return result;
		}
	}
}
