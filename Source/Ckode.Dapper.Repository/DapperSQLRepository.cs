using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Ckode.Dapper.Repository.MetaInformation;
using Ckode.Dapper.Repository.MetaInformation.PropertyInfos;

namespace Ckode.Dapper.Repository
{
    public abstract class DapperSQLRepository<TPrimaryKeyRecord, TRecord> : IRepository<TPrimaryKeyRecord, TRecord>
        where TPrimaryKeyRecord : BaseTableRecord
        where TRecord : TPrimaryKeyRecord
    {
        #region Dapper delegates

        protected delegate T QuerySingleDelegate<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
        protected delegate IEnumerable<T> QueryDelegate<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
        #endregion

        private readonly SQLGenerator generator;

        public DapperSQLRepository()
        {
            generator = new SQLGenerator(TableName);
        }

        protected abstract string TableName { get; }

        protected abstract IDbConnection CreateConnection();

        #region Injection points for Dapper methods

        protected abstract QuerySingleDelegate<TRecord> QuerySingle { get; }

        protected abstract QuerySingleDelegate<TRecord> QuerySingleOrDefault { get; }

        protected abstract QueryDelegate<TRecord> Query { get; }

        #endregion

        public TRecord Delete(TPrimaryKeyRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var info = RecordInformationCache.GetRecordInformation<TRecord>();

            CheckForDefaultPrimaryKeys(info, record);

            var query = generator.GenerateDeleteQuery<TRecord>();
            var result = ExecuteQueryOrDefault(record, query);

            return ReturnOrThrowIfRecordIsNull(record, info, result, query);
        }

        public TRecord Get(TPrimaryKeyRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var info = RecordInformationCache.GetRecordInformation<TRecord>();

            CheckForDefaultPrimaryKeys(info, record);

            var query = generator.GenerateGetQuery<TRecord>();
            var result = ExecuteQueryOrDefault(record, query);

            return ReturnOrThrowIfRecordIsNull(record, info, result, query);
        }
        public IEnumerable<TRecord> GetAll()
        {
            var query = generator.GenerateGetAllQuery<TRecord>();
            using var connection = CreateConnection();
            return Query.Invoke(connection, query);
        }

        public TRecord Insert(TRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var info = RecordInformationCache.GetRecordInformation<TRecord>();

            var invalidIdentityColumns = info.PrimaryKeys
                                                .Where(pk => pk.IsIdentity && !pk.HasDefaultValue(record))
                                                .ToList();

            if (invalidIdentityColumns.Any())
            {
                throw new ArgumentException($"record has the following primary keys marked with IsIdentity, which have non-default values: {string.Join(", ", invalidIdentityColumns.Select(col => col.Name))}", nameof(record));
            }

            var query = generator.GenerateInsertQuery<TRecord>();
            using var connection = CreateConnection();
            return QuerySingle.Invoke(connection, query, record);
        }

        public TRecord Update(TRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var info = RecordInformationCache.GetRecordInformation<TRecord>();

            CheckForDefaultPrimaryKeys(info, record);

            var query = generator.GenerateUpdateQuery<TRecord>();
            var result = ExecuteQueryOrDefault(record, query);

            return ReturnOrThrowIfRecordIsNull(record, info, result, query);
        }


        private static void CheckForDefaultPrimaryKeys(RecordInformation info, TPrimaryKeyRecord record)
        {
            var invalidPrimaryKeys = info.PrimaryKeys
                                               .Where(pk => pk.HasDefaultValue(record))
                                               .ToList();

            if (invalidPrimaryKeys.Any())
            {
                throw new ArgumentException($"record has the following primary keys which have default values: {string.Join(", ", invalidPrimaryKeys.Select(col => col.Name))}", nameof(record));
            }
        }

        private static TRecord ReturnOrThrowIfRecordIsNull(TPrimaryKeyRecord record, RecordInformation info, TRecord? result, string query)
        {
            if (result == null)
            {
                var formattedPrimaryKeys = PrintPrimaryKeys(info.PrimaryKeys, record);
                throw new NoRecordFoundException(query, formattedPrimaryKeys, $"No matching record found with the given primary keys.");
            }
            return result;
        }

        private TRecord? ExecuteQueryOrDefault(TPrimaryKeyRecord record, string query)
        {
            using var connection = CreateConnection();
            return QuerySingleOrDefault.Invoke(connection, query, record);
        }


        private static string PrintPrimaryKeys(IReadOnlyCollection<PrimaryKeyPropertyInfo> primaryKeys, TPrimaryKeyRecord record)
        {
            return string.Join(Environment.NewLine, primaryKeys.Select(pk => $"{pk.Name} = {FormatPrimaryKeyValue(pk, record)}"));
        }

        private static string FormatPrimaryKeyValue(PrimaryKeyPropertyInfo primaryKey, TPrimaryKeyRecord record)
        {
            var valueAsString = primaryKey.GetValue(record)?.ToString() ?? "null";
            if (primaryKey.Type == typeof(string))
                return $@"""{valueAsString}""";
            else
                return valueAsString;
        }
    }
}
