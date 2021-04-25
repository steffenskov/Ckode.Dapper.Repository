using System;
using System.Threading.Tasks;
using Ckode.Dapper.Repository.IntegrationTests.Entities;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class CompositePrimaryKeyAsyncTests
	{
		[Fact]
		public async Task Delete_PrimaryKeyPartiallyNotEntered_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(new CompositeUserPrimaryKeyEntity { Username = "async My name" }));
		}

		[Fact]
		public async Task Delete_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			await Assert.ThrowsAsync<NoEntityFoundException>(async () => await repository.DeleteAsync(new CompositeUserPrimaryKeyEntity { Username = "async My name", Password = "Secret" }));
		}

		[Fact]
		public async Task Delete_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var entity = new CompositeUserEntity
			{
				Username = "async My name1",
				Password = "Secret"
			};
			var insertedEntity = repository.Insert(entity);

			// Act
			var deleted = await repository.DeleteAsync(new CompositeUserPrimaryKeyEntity { Username = "async My name1", Password = "Secret" });

			// Assert
			Assert.Equal(entity.Username, deleted.Username);
			Assert.Equal(entity.Password, deleted.Password);
			Assert.Equal(insertedEntity.DateCreated, deleted.DateCreated);
		}

		[Fact]
		public async Task Get_PrimaryKeyPartiallyNotEntered_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetAsync(new CompositeUserPrimaryKeyEntity { Username = "async My name" }));
		}

		[Fact]
		public async Task Get_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			await Assert.ThrowsAsync<NoEntityFoundException>(async () => await repository.GetAsync(new CompositeUserPrimaryKeyEntity { Username = "async My name", Password = "Secret" }));
		}

		[Fact]
		public async Task Get_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var entity = new CompositeUserEntity
			{
				Username = "async My name2",
				Password = "Secret"
			};
			var insertedEntity = repository.Insert(entity);

			// Act
			var gotten = repository.Get(new CompositeUserPrimaryKeyEntity { Username = "async My name2", Password = "Secret" });

			// Assert
			Assert.Equal(entity.Username, gotten.Username);
			Assert.Equal(entity.Password, gotten.Password);
			Assert.Equal(insertedEntity.DateCreated, gotten.DateCreated);

			await repository.DeleteAsync(insertedEntity);
		}

		[Fact]
		public async Task Update_PrimaryKeyPartiallyNotEntered_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(new CompositeUserEntity { Username = "async My name" }));
		}

		[Fact]
		public async Task Update_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			await Assert.ThrowsAsync<NoEntityFoundException>(async () => await repository.UpdateAsync(new CompositeUserEntity { Username = "Doesnt exist", Password = "Secret" }));
		}

		[Fact]
		public async Task Update_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var entity = new CompositeUserEntity
			{
				Username = "async My name3",
				Password = "Secret"
			};
			var insertedEntity = repository.Insert(entity);

			// Act
			var updated = repository.Update(insertedEntity with { Age = 42 });

			// Assert
			Assert.Equal(entity.Username, updated.Username);
			Assert.Equal(entity.Password, updated.Password);
			Assert.NotEqual(42, insertedEntity.Age);
			Assert.Equal(42, updated.Age);
			Assert.Equal(insertedEntity.DateCreated, updated.DateCreated);

			await repository.DeleteAsync(insertedEntity);
		}
	}
}