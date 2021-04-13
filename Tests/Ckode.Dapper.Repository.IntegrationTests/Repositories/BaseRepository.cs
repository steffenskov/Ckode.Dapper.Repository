using System.Data;
using System.Data.SqlClient;
using SqlMapper = Dapper.SqlMapper;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public abstract class BaseRepository<TRecord> : DapperSQLRepository<TRecord>
		where TRecord : BaseTableRecord
	{
		private const string _connectionString = @"Data Source=.\SQLExpress;Database=Northwind;Integrated Security=true;";

		protected override QuerySingleDelegate<TRecord> QuerySingle => SqlMapper.QuerySingle<TRecord>;
		protected override QuerySingleDelegate<TRecord> QuerySingleOrDefault => SqlMapper.QuerySingleOrDefault<TRecord>;

		protected override IDbConnection CreateConnection()
		{
			return new SqlConnection(_connectionString);
		}
	}
}
