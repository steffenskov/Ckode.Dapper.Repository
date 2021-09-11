using Ckode.Dapper.Repository.IntegrationTests.Entities;

namespace Ckode.Dapper.Repository.IntegrationTests.MySql.Repositories
{
	public class CategoryRepository : MyPrimaryKeyRepository<CategoryPrimaryKeyEntity, CategoryEntity>
	{
		protected override string TableName => "categories";
	}
}
