using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Ckode.Dapper.Repository
{
	public delegate T QuerySingleDelegate<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

	public delegate IEnumerable<T> QueryDelegate<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);

	public delegate Task<IEnumerable<T>> QueryAsyncDelegate<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

	public delegate Task<T> QuerySingleAsyncDelegate<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);

	public interface IDapperInjection<TRecord>
	where TRecord : TableRecord
	{
		QuerySingleDelegate<TRecord> QuerySingle { get; }

		QuerySingleDelegate<TRecord> QuerySingleOrDefault { get; }

		QueryDelegate<TRecord> Query { get; }

		QuerySingleAsyncDelegate<TRecord> QuerySingleAsync { get; }

		QuerySingleAsyncDelegate<TRecord> QuerySingleOrDefaultAsync { get; }

		QueryAsyncDelegate<TRecord> QueryAsync { get; }
	}
}