using System;
using System.Reflection;
using System.Data.SQLite;

using AuthDemo.Exceptions;

namespace AuthDemo.Data
{

	public abstract class ProgramDB<T> : DatabaseAccess<T>
	{
		static Type entityType;
		static PropertyInfo[] entityProperties;
		static PropertyInfo currentProperty;
		static PropertyBorders propertiesForDB;

		static ProgramDB()
		{
			entityType = typeof(T);
			entityProperties = entityType.GetProperties();

			propertiesForDB = new PropertyBorders();
		}

		static protected int FirstPropertyIndex
		{
			get => propertiesForDB.FirstPropertyIndex;
			set => propertiesForDB.FirstPropertyIndex = value;
		}

		static protected int LastPropertyIndex
		{
			get => propertiesForDB.LastPropertyIndex;
			set => propertiesForDB.LastPropertyIndex = value;
		}

		static PropertyInfo[] EntityProperties => entityProperties;

		static string EntityName => entityType.Name;

		static PropertyInfo CurrentProperty
		{
			get => currentProperty;
			set => currentProperty = value;
		}

		static string GetPropertyName(int propertyIndex)
		{
			return EntityProperties[propertyIndex].Name;
		}

		static string GetPropertyValue(int propertyIndex, T entity)
		{
			return EntityProperties[propertyIndex].
				GetValue(entity).ToString();
		}

		static void SetPropertyValue(int propertyIndex, T entity, object value)
		{
			EntityProperties[propertyIndex].SetValue(entity, value);
		}

		static public int Write(T entity)
		{
			try
			{
				TryOpenConnection();

				CurrentQuery = $"INSERT INTO [{EntityName}] (";
				for (int i = FirstPropertyIndex; i <= LastPropertyIndex; i++)
				{
					CurrentQuery += GetPropertyName(i);
					if (i < LastPropertyIndex)
						CurrentQuery += ", ";
					else
						CurrentQuery += ")";
				}

				CurrentQuery += " VALUES (";
				for (int i = FirstPropertyIndex; i <= LastPropertyIndex; i++)
				{
					CurrentProperty = EntityProperties[i];
					AddParameter(GetPropertyName(i), GetPropertyValue(i, entity));
					CurrentQuery += $"@{GetPropertyName(i)}";

					if (i < LastPropertyIndex)
						CurrentQuery += ", ";
				}
				CurrentQuery += ");";

				try { return ExecuteNonQueryCommand(); }
				finally { FinishQuery(); }
			}
			catch (SQLiteException)
			{
				throw new DataAlreadyExistException(
					"Сущность уже существует"
				);
			}
		}

		static public int Edit(T oldEntity, T newEntity)
		{
			TryOpenConnection();

			CurrentQuery = $"UPDATE [{EntityName}] " +
				"SET ";

			for (int i = FirstPropertyIndex; i <= LastPropertyIndex; i++)
			{
				CurrentProperty = EntityProperties[i];
				CurrentQuery += $"{GetPropertyName(i)} = ";
				AddParameter(GetPropertyName(i), GetPropertyValue(i, newEntity));
				CurrentQuery += $"@{GetPropertyName(i)}";

				if (i != LastPropertyIndex)
					CurrentQuery += ", ";
			}

			CurrentQuery += " WHERE ";

			for (int i = FirstPropertyIndex; i <= LastPropertyIndex; i++)
			{
				CurrentProperty = EntityProperties[i];
				CurrentQuery += $"{GetPropertyName(i)} = ";
				AddParameter($"{GetPropertyName(i)}_new", GetPropertyValue(i, oldEntity));
				CurrentQuery += $"@{GetPropertyName(i)}_new";

				if (i != LastPropertyIndex)
					CurrentQuery += " AND ";
			}

			try { return ExecuteNonQueryCommand(); }
			finally { FinishQuery(); }
		}

		static public int Delete(T entity)
		{
			TryOpenConnection();

			CurrentQuery = $"DELETE FROM [{EntityName}] " +
				"WHERE ";

			for (int i = FirstPropertyIndex; i <= LastPropertyIndex; i++)
			{
				CurrentProperty = EntityProperties[i];
				CurrentQuery += $"{GetPropertyName(i)} = ";
				AddParameter($"{GetPropertyName(i)}", GetPropertyValue(i, entity));
				CurrentQuery += $"@{GetPropertyName(i)}";

				if (i != LastPropertyIndex)
					CurrentQuery += " AND ";
			}

			try { return ExecuteNonQueryCommand(); }
			finally { FinishQuery(); }
		}
	}
}
