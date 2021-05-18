using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ckode.Dapper.Repository.Sql
{
	/// <summary>
	/// Base repository, don't inherit this class, but rather use PrimaryKeyRepository or HeapRepository.
	/// </summary>
	public abstract class DbRepository<TEntity>
	where TEntity : DbEntity
	{
		protected string Schema => "dbo";

		protected abstract IDbConnection CreateConnection();

		protected abstract IDapperInjection<T> CreateDapperInjection<T>();

		private readonly IDapperInjection<TEntity> _dapperInjection;


		protected DbRepository()
		{
			_dapperInjection = CreateDapperInjection<TEntity>();
		}

		#region Query
		protected IEnumerable<TEntity> Query(string query, object? param = null, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return _dapperInjection.Query.Invoke(connection, query, param, transaction, buffered, commandTimeout, commandType);
		}

		protected async Task<IEnumerable<TEntity>> QueryAsync(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return await _dapperInjection.QueryAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected TEntity? QuerySingleOrDefault(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return _dapperInjection.QuerySingleOrDefault.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected async Task<TEntity?> QuerySingleOrDefaultAsync(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return await _dapperInjection.QuerySingleOrDefaultAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected TEntity QuerySingle(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return _dapperInjection.QuerySingle.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected async Task<TEntity> QuerySingleAsync(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return await _dapperInjection.QuerySingleAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected IEnumerable<TResult> ScalarMultiple<TResult>(string query, object? param, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			var dapperInjection = CreateDapperInjection<TResult>();
			return dapperInjection.Query.Invoke(connection, query, param, transaction, buffered, commandTimeout, commandType);
		}

		protected async Task<IEnumerable<TResult>> ScalarMultipleAsync<TResult>(string query, object? param, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			var dapperInjection = CreateDapperInjection<TResult>();
			return await dapperInjection.QueryAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected TResult? ScalarSingleOrDefault<TResult>(string query, object? param, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			var dapperInjection = CreateDapperInjection<TResult>();
			return dapperInjection.QuerySingleOrDefault.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected async Task<TResult?> ScalarSingleOrDefaultAsync<TResult>(string query, object? param, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			var dapperInjection = CreateDapperInjection<TResult>();
			return await dapperInjection.QuerySingleOrDefaultAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected TResult ScalarSingle<TResult>(string query, object? param, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			var dapperInjection = CreateDapperInjection<TResult>();
			return dapperInjection.QuerySingle.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected async Task<TResult> ScalarSingleAsync<TResult>(string query, object? param, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			var dapperInjection = CreateDapperInjection<TResult>();
			return await dapperInjection.QuerySingleAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected int Execute(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return _dapperInjection.Execute.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}

		protected async Task<int> ExecuteAsync(string query, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
		{
			using var connection = CreateConnection();
			return await _dapperInjection.ExecuteAsync.Invoke(connection, query, param, transaction, commandTimeout, commandType);
		}
	}
	#endregion
}