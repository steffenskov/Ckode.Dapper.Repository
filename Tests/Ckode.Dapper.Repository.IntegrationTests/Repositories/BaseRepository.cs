using System.Data;
using System.Data.SqlClient;
using SqlMapper = Dapper.SqlMapper;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
    public abstract class BaseRepository<TPrimaryKeyRecord, TRecord> : DapperSQLRepository<TPrimaryKeyRecord, TRecord>
        where TPrimaryKeyRecord : BaseTableRecord
        where TRecord : TPrimaryKeyRecord
    {
        private const string _connectionString = @"Server=localhost;Database=Northwind;User Id=sa;Password=SqlServer2019;";

        protected override QuerySingleDelegate<TRecord> QuerySingle => SqlMapper.QuerySingle<TRecord>;
        protected override QuerySingleDelegate<TRecord> QuerySingleOrDefault => SqlMapper.QuerySingleOrDefault<TRecord>;
        protected override QueryDelegate<TRecord> Query => SqlMapper.Query<TRecord>;

        protected override IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
