using Ckode.Dapper.Repository.IntegrationTests.Records;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class CompositeUserRepository : MyPrimaryKeyRepository<CompositeUserPrimaryKeyRecord, CompositeUserRecord>
	{
		protected override string TableName => "CompositeUsers";

	}
}
