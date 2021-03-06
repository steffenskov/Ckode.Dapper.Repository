using System.Linq;
using Ckode.Dapper.Repository.IntegrationTests.Sql.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests.Sql
{
	public class ViewTests
	{
		private readonly ProductListViewRepository _repository;

		public ViewTests()
		{
			_repository = new ProductListViewRepository();
		}

		[Fact]
		public void GetAll_HaveRows_Valid()
		{
			// Act
			var all = _repository.GetAll();

			Assert.True(all.Count() >= 2);
		}
	}
}
