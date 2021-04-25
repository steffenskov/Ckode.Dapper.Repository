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
			var entity = new CompositeUserEntity
			{
				Username = "My name1",
				Password = "Secret"
			};

			// Act
			var insertedEntity = repository.Insert(entity);

			// Assert
			try
			{
				Assert.Equal(entity.Username, insertedEntity.Username);
				Assert.Equal(entity.Password, insertedEntity.Password);
				Assert.True(insertedEntity.DateCreated > DateTime.UtcNow.AddHours(-1));
			}
			finally
			{
				repository.Delete(entity);
			}
		}

		[Fact]
		public void Update_ColumnHasMissingSetter_ColumnIsExcluded()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var entity = new CompositeUserEntity
			{
				Username = "My name2",
				Password = "Secret"
			};

			// Act
			var insertedEntity = repository.Insert(entity);

			// Assert
			try
			{
				Assert.Equal(entity.Username, insertedEntity.Username);
				Assert.Equal(entity.Password, insertedEntity.Password);
				Assert.True(insertedEntity.DateCreated > DateTime.UtcNow.AddHours(-1));
			}
			finally
			{
				repository.Delete(entity);
			}
		}
	}
}