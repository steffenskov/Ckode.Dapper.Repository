using Ckode.Dapper.Repository.IntegrationTests.Entities;

namespace Ckode.Dapper.Repository.IntegrationTests.Sql.Repositories
{
	public class CompositeUserRepository : MyPrimaryKeyRepository<CompositeUserPrimaryKeyEntity, CompositeUserEntity>
	{
		protected override string TableName => "CompositeUsers";

	}
}
