using Ckode.Dapper.Repository.IntegrationTests.Entitys;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class CategoryRepository : MyPrimaryKeyRepository<CategoryPrimaryKeyEntity, CategoryEntity>
	{
		protected override string TableName => "Categories";

	}
}
