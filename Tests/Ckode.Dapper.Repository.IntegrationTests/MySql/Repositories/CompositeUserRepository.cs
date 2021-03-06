using Ckode.Dapper.Repository.IntegrationTests.Entities;

namespace Ckode.Dapper.Repository.IntegrationTests.MySql.Repositories
{
	public class CompositeUserRepository : MyPrimaryKeyRepository<CompositeUserPrimaryKeyEntity, CompositeUserEntity>
	{
		protected override string TableName => "composite_users";

	}
}
