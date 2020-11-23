using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	/// <summary>
	///	Class that represents user from database
	/// </summary>
	public class User : ProgramDB<User>
	{
		Group group;

		public User()
		{
			group = new Group();

			FirstPropertyIndex = 1;
			LastPropertyIndex = 6;
		}

		public long ID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string MiddleName { get; set; }

		public string Login { get; set; }

		public string Password { get; set; }

		public long GroupID => Group.ID;

		public Group Group { get; set; }

		public string FullName => $"{LastName} {FirstName} {MiddleName}";

		static public User GetUserByLogin(string login)
		{
			try
			{
				TryOpenConnection();

				CurrentQuery = "SELECT * FROM User " +
					$"WHERE Login = @login;";

				AddParameter("@login", login);

				ExecuteReader();

				if (Reader.HasRows)
				{
					SwitchToNextRow();
					return GetEntityFromReader();
				}
				else
				{
					throw new NoSuchDataException(
						"Пользователь не найден"
					);
				}
			}
			finally
			{
				FinishQuery();
			}
		}

		static public List<User> GetAllEntities()
		{
			TryOpenConnection();

			var result = new List<User>();

			try
			{
				CurrentQuery =
					$"SELECT * FROM [User];";
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

		static User GetEntityFromReader()
		{
			var result = new User();

			result.ID = (long)ReadColumnFromRow(0);
			result.LastName = (string)ReadColumnFromRow(1);
			result.FirstName = (string)ReadColumnFromRow(2);
			result.MiddleName = (string)ReadColumnFromRow(3);
			result.Login = (string)ReadColumnFromRow(4);
			result.Password = (string)ReadColumnFromRow(5);
			result.Group = Group.GetGroupByID((long)ReadColumnFromRow(6));

			return result;
		}
	}
}
