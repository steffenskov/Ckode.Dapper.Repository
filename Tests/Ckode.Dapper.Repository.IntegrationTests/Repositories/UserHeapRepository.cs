using Ckode.Dapper.Repository.IntegrationTests.Records;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class UserHeapRepository : MyHeapRepository<UserHeapRecord>
	{
		protected override string TableName => "Heaps";
	}
}