using Ckode.Dapper.Repository.IntegrationTests.Records;

namespace Ckode.Dapper.Repository.IntegrationTests.Repositories
{
	public class CategoryRepository : BaseRepository<CategoryRecord>
	{
		protected override string TableName => "Categories";
	}
}
