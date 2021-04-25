using System;
using Ckode.Dapper.Repository.IntegrationTests.Entities;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class OtherTests
	{
		[Fact]
		public void Insert_RelyOnDefaultConstraint_Valid()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var record = new CompositeUserEntity
			{
				Username = "My name1",
				Password = "Secret"
			};

			// Act
			var insertedEntity = repository.Insert(record);

			// Assert
			try
			{
				Assert.Equal(record.Username, insertedEntity.Username);
				Assert.Equal(record.Password, insertedEntity.Password);
				Assert.True(insertedEntity.DateCreated > DateTime.UtcNow.AddHours(-1));
			}
			finally
			{
				repository.Delete(record);
			}
		}

		[Fact]
		public void Update_ColumnHasMissingSetter_ColumnIsExcluded()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var record = new CompositeUserEntity
			{
				Username = "My name2",
				Password = "Secret"
			};

			// Act
			var insertedEntity = repository.Insert(record);

			// Assert
			try
			{
				Assert.Equal(record.Username, insertedEntity.Username);
				Assert.Equal(record.Password, insertedEntity.Password);
				Assert.True(insertedEntity.DateCreated > DateTime.UtcNow.AddHours(-1));
			}
			finally
			{
				repository.Delete(record);
			}
		}
	}
}