using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ckode.Dapper.Repository.Sql
{
	/// <summary>
	/// Base repository, don't inherit this class, but rather use PrimaryKeyRepository or HeapRepository.
	/// </summary>
	public abstract class BaseRepository<TEntity>
	where TEntity : TableEntity
	{
		protected abstract string TableName { get; }

		protected string Schema => "dbo";

		protected string FormattedTableName => $"[{Schema}].[{TableName}]";

		protected abstract IDbConnection CreateConnection();

		protected abstract IDapperInjection<TEntity> DapperInjection { get; }

		#region Query
		protected IEnumerable<TEntity> Query(string query, object? param = null, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return DapperInjection.Query.Invoke(connection, query, param, transaction, buffered, commandTimeout, commandType);
		}

		protected async Task<IEnumerable<TEntity>> QueryAsync(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return await DapperInjection.QueryAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected TEntity? QuerySingleOrDefault(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return DapperInjection.QuerySingleOrDefault.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected async Task<TEntity?> QuerySingleOrDefaultAsync(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return await DapperInjection.QuerySingleOrDefaultAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected TEntity QuerySingle(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return DapperInjection.QuerySingle.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected async Task<TEntity> QuerySingleAsync(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return await DapperInjection.QuerySingleAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}
		#endregion

	}
}