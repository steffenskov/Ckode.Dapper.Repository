using Ckode.Dapper.Repository.IntegrationTests.Records;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class CategoryRepository : MyPrimaryKeyRepository<CategoryPrimaryKeyRecord, CategoryRecord>
	{
		protected override string TableName => "Categories";

	}
}
