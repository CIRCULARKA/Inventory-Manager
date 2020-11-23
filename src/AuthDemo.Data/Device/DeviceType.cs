using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class DeviceType : ProgramDB<DeviceType>
	{
		public DeviceType()
		{
			FirstPropertyIndex = 0;
			LastPropertyIndex = 1;
		}

		public long ID { get; set; }

		public string Name { get; set; }

		static public DeviceType GetDeviceTypeByName(string name)
		{
			TryOpenConnection();

			var result = new DeviceType();

			CurrentQuery =
				$"SELECT * FROM DeviceType WHERE Name = @name;";

			AddParameter("@name", name);

			ExecuteReader();
			SwitchToNextRow();

			result.ID = (int)ReadColumnFromRow(0);
			result.Name = (string)ReadColumnFromRow(1);

			FinishQuery();

			return result;
		}

		static public DeviceType GetDeviceTypeByID(long id)
		{
			TryOpenConnection();

			var result = new DeviceType();

			CurrentQuery =
				$"SELECT * FROM [DeviceType] WHERE ID = @id;";

			AddParameter("@id", id);

			ExecuteReader();
			SwitchToNextRow();

			result.ID = (long)ReadColumnFromRow(0);
			result.Name = (string)ReadColumnFromRow(1);

			FinishQuery();

			return result;
		}

		static DeviceType GetEntityFromReader()
		{
			var result = new DeviceType();
			result.ID = (long)ReadColumnFromRow(0);
			result.Name = (string)ReadColumnFromRow(1);

			return result;
		}

		static public List<DeviceType> GetAll()
		{
			TryOpenConnection();

			var result = new List<DeviceType>();

			try
			{
				CurrentQuery =
					$"SELECT * FROM [DeviceType];";
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

		public override string ToString() => Name;
	}
}
