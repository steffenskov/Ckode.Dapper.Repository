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

        public static RecordInformation GetRecordInformation<TRecord>()
            where TRecord : TableRecord
        {
            var type = typeof(TRecord);
            if (!_cache.TryGetValue(type, out var result))
            {
                lock (_cacheLock)
                {
                    if (!_cache.TryGetValue(type, out result))
                    {
                        _cache[type] = result = CreateRecordInformation<TRecord>();
                    }
                }
            }
            return result;
        }

        private static RecordInformation CreateRecordInformation<TRecord>()
            where TRecord : TableRecord
        {
            var properties = TypeCache.GetProperties<TRecord>();
            var primaryKeys = new List<PrimaryKeyPropertyInfo>();
            var foreignKeys = new List<ForeignKeyPropertyInfo>();
            var columns = new List<ColumnPropertyInfo>();

            foreach (var prop in properties)
            {
                var column = prop.GetCustomAttribute<ColumnAttribute>();
                if (column == null)
                {
                    continue;
                }

                columns.Add(new ColumnPropertyInfo(prop, column));
                var primaryKey = prop.GetCustomAttribute<PrimaryKeyAttribute>();
                var foreignKey = prop.GetCustomAttribute<ForeignKeyAttribute>();

                if (primaryKey != null)
                {
                    primaryKeys.Add(new PrimaryKeyPropertyInfo(prop, primaryKey));
                }
                if (foreignKey != null)
                {
                    foreignKeys.Add(new ForeignKeyPropertyInfo(prop, foreignKey));
                }
            }

            return new RecordInformation(primaryKeys.AsReadOnly(), foreignKeys.AsReadOnly(), columns.AsReadOnly());
        }
    }
}
