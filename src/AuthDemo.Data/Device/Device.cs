using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class Device : ProgramDB<Device>
	{
		public Device()
		{
			FirstPropertyIndex = 0;
			LastPropertyIndex = 3;
		}

		public long SerialNumber { get; set; }

		public string InventoryNumber { get; set; }

		public long TypeID => Type.ID;

		public string NetworkName { get; set; }

		public DeviceType Type { get; set; }

		static public List<Device> GetAll()
		{
			TryOpenConnection();

			var result = new List<Device>();

			try
			{
				CurrentQuery =
					$"SELECT * FROM [Device];";
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

		static public Device GetDeviceBySerialNumber(long serialNumber)
		{
			try
			{
				TryOpenConnection();

				CurrentQuery = "SELECT * FROM Device " +
					$"WHERE SerialNumber = @serialNumber;";

				AddParameter("@serialNumber", serialNumber);

				ExecuteReader();

				if (Reader.HasRows)
				{
					SwitchToNextRow();
					return GetEntityFromReader();
				}
				else
				{
					throw new NoSuchDataException(
						"Устройство не найдено"
					);
				}
			}
			finally { FinishQuery(); }
		}

		static public List<Device> GetAllWithoutIP()
		{
			TryOpenConnection();

			var result = new List<Device>();

			try
			{
				CurrentQuery =
					$"SELECT SerialNumber, InventoryNumber, TypeID, NetworkName FROM Device, DeviceConfiguration " +
						"WHERE DeviceConfiguration.IPAddressID = 1 AND " +
						"Device.SerialNumber = DeviceConfiguration.DeviceSerialNumber;";

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

		static protected Device GetEntityFromReader()
		{
			var result = new Device();
			result.SerialNumber = (long)ReadColumnFromRow(0);
			result.InventoryNumber = (string)ReadColumnFromRow(1);
			result.Type = DeviceType.GetDeviceTypeByID((long)ReadColumnFromRow(2));
			result.NetworkName = (string)ReadColumnFromRow(3);

			return result;
		}
	}
}
