using System;
using System.Collections.Generic;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public class Sertificate : ProgramDB<Sertificate>
	{
		public Sertificate()
		{
			FirstPropertyIndex = 0;
			LastPropertyIndex = 3;
		}

		public string SubjectName { get; set; }

		public long SerialNumber { get; set; }

		public string ValidFrom
			=> $"{ValidFromDateTime.Year}-{ValidFromDateTime.Month}-{ValidFromDateTime.Day}";

		public string ValidUntil
			=> $"{ValidUntilDateTime.Year}-{ValidUntilDateTime.Month}-{ValidUntilDateTime.Day}";

		public DateTime ValidFromDateTime { get; set; }

		public DateTime ValidUntilDateTime { get; set; }

		public string ValidFromCorrectFormat
			=> $"{ValidFromDateTime.Day}.{ValidFromDateTime.Month}.{ValidFromDateTime.Year}";

		public string ValidUntilCorrectFormat
			=> $"{ValidUntilDateTime.Day}.{ValidUntilDateTime.Month}.{ValidUntilDateTime.Year}";

		static string ExtendDatePart(string value)
		{
			if (value.Length < 2)
				return "0" + value;
			return value;
		}

		static public List<Sertificate> GetAllEntities()
		{
			TryOpenConnection();

			var result = new List<Sertificate>();

			try
			{
				CurrentQuery =
					$"SELECT * FROM [Sertificate];";
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

		static Sertificate GetEntityFromReader()
		{
			var result = new Sertificate();

			result.SubjectName = (string)ReadColumnFromRow(0);
			result.SerialNumber = (long)ReadColumnFromRow(1);

			string stringDate = (string)ReadColumnFromRow(2);
			string[] curDates = stringDate.Split('.', '\\', '/', '-');
			result.ValidFromDateTime = DateTime.Parse(
				$"{curDates[2]}-{ExtendDatePart(curDates[1])}-" +
				$"{ExtendDatePart(curDates[0])}"
			);

			stringDate = (string)ReadColumnFromRow(3);
			curDates = stringDate.Split('.', '\\', '/', '-');
			result.ValidUntilDateTime = DateTime.Parse(
				$"{curDates[2]}-{ExtendDatePart(curDates[1])}-" +
				$"{ExtendDatePart(curDates[0])}"
			);

			return result;
		}
	}
}
