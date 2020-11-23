using System.Data.SQLite;
using System.Configuration;
using System.Collections.Generic;
using System.Reflection;
using System;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{
	public abstract class DatabaseAccess<T>
	{
		static private readonly SQLiteConnection connection;
		static private readonly SQLiteCommand command;
		static private SQLiteDataReader reader;

		static DatabaseAccess()
		{
			connection = new SQLiteConnection()
			{
				ConnectionString =
					ConfigurationManager.
					ConnectionStrings["defaultConnection"].
					ConnectionString
			};

			command = new SQLiteCommand()
			{
				Connection = connection
			};
		}

		static protected string CurrentQuery
		{
			get => Command.CommandText;
			set => Command.CommandText = value;
		}

		static protected SQLiteCommand Command => command;
		static protected SQLiteDataReader Reader
		{
			get => reader;
			set => reader = value;
		}

		/// <summary>
		///	Exception: InvalidOperationException
		/// </summary>
		static protected void TryOpenConnection()
		{
			try
			{
				connection.Open();
				// CurrentQuery = "PRAGMA foreign_keys = ON;";
				// ExecuteNonQueryCommand();
			}
			catch (InvalidOperationException) { }
		}

		static protected void CloseConnection() { connection.Close(); }

		static protected void ExecuteReader() { reader = command.ExecuteReader(); }

		/// <summary>
		/// Returns true if there is row to read
		/// </summary>
		static protected bool SwitchToNextRow() { return reader.Read(); }

		static protected object ReadColumnFromRow(int column) { return reader.GetValue(column); }

		static protected void CloseReader()
		{
			try { reader.Close(); }
			catch (NullReferenceException) { }
		}

		/// <summary>
		/// Returns number of affected rows
		/// </summary>
		static protected int ExecuteNonQueryCommand() { return command.ExecuteNonQuery(); }

		static protected void AddParameter(string paramName, object value)
		{
			Command.Parameters.Add(
				new SQLiteParameter(paramName, value)
			);
		}

		static protected void ResetCommand() { command.Reset(); }

		static protected void FinishQuery()
		{
			ResetCommand();
			CloseReader();
			CloseConnection();
		}
	}
}
