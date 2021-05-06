using System;
using System.Linq;
using Ckode.Dapper.Repository.IntegrationTests.Entities;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class ViewTests
	{
		[Fact]
		public void GetAll_HaveRows_Valid()
		{
			// Arrange
			var repository = new ProductListViewRepository();

			// Act
			var all = repository.GetAll();

			Assert.True(all.Count() >= 2);
		}
	}
}
