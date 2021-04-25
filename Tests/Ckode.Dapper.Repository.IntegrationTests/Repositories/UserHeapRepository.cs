using Ckode.Dapper.Repository.IntegrationTests.Entities;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class UserHeapRepository : MyHeapRepository<UserHeapEntity>
	{
		protected override string TableName => "Heaps";
	}
}