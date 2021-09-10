using System.Data;
using System.Data.SqlClient;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public static class ConnectionFactory
	{
		private const string _sqlConnectionString = @"Server=localhost;Database=Northwind;User Id=sa;Password=SqlServer2019;";

		public static IDbConnection CreateSqlConnection()
		{
			return new SqlConnection(_sqlConnectionString);
		}

		public static IDbConnection CreateMySqlConnection()
		{
			return null!;
		}
	}
}
