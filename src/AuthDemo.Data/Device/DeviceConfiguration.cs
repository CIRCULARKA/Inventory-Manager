using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class DeviceConfiguration : ProgramDB<DeviceConfiguration>
	{
		static DeviceConfiguration()
		{
			FirstPropertyIndex = 0;
			LastPropertyIndex = 3;
		}

		public long DeviceSerialNumber { get; set; }

		public long IPAddressID => IP.ID;

		public string AccountPassword { get; set; }

		public string AccountName { get; set; }

		public IPAddress IP { get; set; }

		static public DeviceConfiguration GetDeviceConfiguration(Device device)
		{
			try
			{
				TryOpenConnection();

				CurrentQuery = "SELECT * FROM DeviceConfiguration " +
					"WHERE DeviceSerialNumber = @serialNumber;";

				AddParameter("@serialNumber", device.SerialNumber);

				ExecuteReader();

				if (Reader.HasRows)
				{
					SwitchToNextRow();
					return GetEntityFromReader();
				}
				else
				{
					throw new NoSuchDataException(
						"Устройство не настроено"
					);
				}
			}
			finally { FinishQuery(); }
		}

		private static DeviceConfiguration GetEntityFromReader()
		{
			var result = new DeviceConfiguration()
			{
				DeviceSerialNumber = (long)ReadColumnFromRow(0),
				IP = IPAddress.GetIPByID((long)ReadColumnFromRow(1)),
				AccountName = (string)ReadColumnFromRow(2),
				AccountPassword = (string)ReadColumnFromRow(3)
			};

			return result;
		}
	}
}
