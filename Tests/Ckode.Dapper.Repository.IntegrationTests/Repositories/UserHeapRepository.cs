using Ckode.Dapper.Repository.IntegrationTests.Entitys;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class UserHeapRepository : MyHeapRepository<UserHeapEntity>
	{
		protected override string TableName => "Heaps";
	}
}