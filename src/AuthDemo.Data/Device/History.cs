using System;
using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class History : ProgramDB<History>
	{
		static History()
		{
			FirstPropertyIndex = 0;
			LastPropertyIndex = 5;
		}

		public long ID { get; set; }

		public long DeviceSerialNumber { get; set; }

		public long CorpsID => Corps.ID;

		public long CabinetID => Cabinet.ID;

		public string ChangeDate =>
			$"{ChangeTimeDateTime.Year}-{ChangeTimeDateTime.Month}-{ChangeTimeDateTime.Day}";

		public long StatusID => Status.ID;

		public Cabinet Cabinet { get; set; }

		public Status Status { get; set; }

		public Corps Corps { get; set; }


		public DateTime ChangeTimeDateTime { get; set; }

		static public List<History> GetDeviceHistory(Device device)
		{
			var result = new List<History>();

			try
			{
				TryOpenConnection();

				CurrentQuery = "SELECT * FROM History " +
					"WHERE DeviceSerialNumber = @serialNumber;";

				AddParameter("@serialNumber", device.SerialNumber);

				ExecuteReader();

				if (Reader.HasRows)
				{
					for (int i = 0; SwitchToNextRow(); i++)
						result.Add(GetEntityFromReader());
					return result;
				}
				else
				{
					throw new NoSuchDataException(
						"С устройством не было совершено транзакций"
					);
				}
			}
			finally { FinishQuery(); }
		}

		static public History GetDeviceLastHistoryNote(Device device)
		{
			var result = GetDeviceHistory(device);
			return result[0];
		}

		public static void DeleteAllDeviceHistory(Device device)
		{
			var deviceHistory = History.GetDeviceHistory(device);

			foreach (History note in deviceHistory)
				History.Delete(note);
		}

		private static History GetEntityFromReader()
		{
			var result = new History();

			result.DeviceSerialNumber = (long)ReadColumnFromRow(0);
			result.Corps = Corps.GetCorpsByID((long)ReadColumnFromRow(1));
			result.Cabinet = Cabinet.GetCabinetByID(
				(long)ReadColumnFromRow(2)
			);

			string stringDate = (string)ReadColumnFromRow(3);
			string[] splittedDate = stringDate.Split('.', '\\', '/', '-');
			result.ChangeTimeDateTime = DateTime.Parse(
				$"{splittedDate[2]}-{splittedDate[1]}-{splittedDate[0]}"
			);

			result.Status = Status.GetStatusByID((long)ReadColumnFromRow(4));

			return result;
		}
	}
}
