using Ckode.Dapper.Repository.IntegrationTests.Records;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class CompositeUserRepository : MyDapperRepository<CompositeUserPrimaryKeyRecord, CompositeUserRecord>
	{
		protected override string TableName => "CompositeUsers";

	}
}
