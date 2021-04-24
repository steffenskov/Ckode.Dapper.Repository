using Ckode.Dapper.Repository.IntegrationTests.Entitys;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class CompositeUserRepository : MyPrimaryKeyRepository<CompositeUserPrimaryKeyEntity, CompositeUserEntity>
	{
		protected override string TableName => "CompositeUsers";

	}
}
