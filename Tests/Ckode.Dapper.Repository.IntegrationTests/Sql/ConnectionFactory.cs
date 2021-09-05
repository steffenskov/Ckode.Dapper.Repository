using System.Data;
using System.Data.SqlClient;

namespace Ckode.Dapper.Repository.IntegrationTests.Sql
{
	public static class ConnectionFactory
	{
		private const string _connectionString = @"Server=localhost;Database=Northwind;User Id=sa;Password=SqlServer2019;";

		public static IDbConnection CreateConnection()
		{
			return new SqlConnection(_connectionString);
		}
	}
}
