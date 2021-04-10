using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Ckode.Dapper.Repository.Attributes;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

namespace Ckode.Dapper.Repository.MetaInformation
{
	internal static class RecordInformationCache
	{
		private static readonly ConcurrentDictionary<Type, RecordInformation> _cache;
		private static readonly object _cacheLock = new object();

		static RecordInformationCache()
		{
			_cache = new ConcurrentDictionary<Type, RecordInformation>();
		}

		public static RecordInformation GetRecordInformation<TRecord>(TRecord record)
			where TRecord : BaseTableRecord
		{
			var type = typeof(TRecord);
			if (!_cache.TryGetValue(type, out var result))
			{
				lock (_cacheLock)
				{
					if (!_cache.TryGetValue(type, out result))
					{
						_cache[type] = result = CreateRecordInformation(record);
					}
				}
			}
			return result;
		}

		private static RecordInformation CreateRecordInformation<TRecord>(TRecord record)
			where TRecord : BaseTableRecord
		{
			var properties = TypeCache.GetProperties(record);
			var primaryKeys = new List<PrimaryKeyPropertyInfo>();
			var foreignKeys = new List<ForeignKeyPropertyInfo>();
			var otherColumns = new List<NormalPropertyInfo>();

			foreach (var prop in properties)
			{
				if (prop.GetCustomAttribute<PrimaryKeyAttribute>() != null)
				{
					primaryKeys.Add(new PrimaryKeyPropertyInfo(prop));
				}
				else if (prop.GetCustomAttribute<ForeignKeyAttribute>() != null)
				{
					foreignKeys.Add(new ForeignKeyPropertyInfo(prop));
				}
				else if (prop.Name != nameof(record.TableName))
				{
					otherColumns.Add(new NormalPropertyInfo(prop));
				}
			}

			return new RecordInformation(primaryKeys.AsReadOnly(), foreignKeys.AsReadOnly(), otherColumns.AsReadOnly());
		}
	}
}
