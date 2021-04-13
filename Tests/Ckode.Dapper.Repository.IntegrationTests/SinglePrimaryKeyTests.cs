using System;
using Ckode.Dapper.Repository.IntegrationTests.Records;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class SinglePrimaryKeyTests
	{
		[Fact]
		public void Insert_HasIdentityKeyWithValue_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				CategoryId = 42,
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};

			// Act && Assert
			Assert.Throws<ArgumentException>(() => repository.Insert(record));
		}


		[Fact]
		public void Insert_HasIdentityKeyWithoutValue_IsInserted()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};

			// Act
			var insertedRecord = repository.Insert(record);
			try
			{
				// Assert
				Assert.NotEqual(default, insertedRecord.CategoryId);
			}
			finally
			{
				repository.Delete(insertedRecord);
			}
		}
	}
}
