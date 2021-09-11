using Ckode.Dapper.Repository.IntegrationTests.Entities;

namespace Ckode.Dapper.Repository.IntegrationTests.MySql.Repositories
{
	public class UserHeapRepository : MyHeapRepository<UserHeapEntity>
	{
		protected override string TableName => "heaps";
	}
}