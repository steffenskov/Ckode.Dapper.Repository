using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Ckode.Dapper.Repository.IntegrationTests.Entities;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class SinglePrimaryKeyAsyncTests
	{
		#region Delete
		[Fact]
		public async Task Delete_InputIsNull_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.DeleteAsync(null!));
		}

		[Fact]
		public async Task Delete_PrimaryKeyNotEntered_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Hello world"
			};

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentException>(async () => await repository.DeleteAsync(entity));
		}

		[Fact]
		public async Task Delete_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			await Assert.ThrowsAsync<NoEntityFoundException>(async () => await repository.DeleteAsync(new CategoryPrimaryKeyEntity { CategoryId = int.MaxValue }));
		}

		[Fact]
		public async Task Delete_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};
			var insertedEntity = await repository.InsertAsync(entity);

			// Act
			var deleted = await repository.DeleteAsync(new CategoryPrimaryKeyEntity { CategoryId = insertedEntity.CategoryId });

			// Assert
			Assert.Equal(insertedEntity.CategoryId, deleted.CategoryId);
			Assert.Equal(entity.Description, deleted.Description);
			Assert.Equal(entity.Name, deleted.Name);
			Assert.Equal(entity.Picture, deleted.Picture);
		}

		[Fact]
		public async Task Delete_UseEntity_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};
			var insertedEntity = await repository.InsertAsync(entity);

			// Act
			var deleted = await repository.DeleteAsync(insertedEntity);

			// Assert
			Assert.Equal(insertedEntity.CategoryId, deleted.CategoryId);
			Assert.Equal(entity.Description, deleted.Description);
			Assert.Equal(entity.Name, deleted.Name);
			Assert.Equal(entity.Picture, deleted.Picture);
		}
		#endregion

		#region Get
		[Fact]
		public async Task Get_InputIsNull_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.GetAsync(null!));
		}

		[Fact]
		public async Task Get_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};
			var insertedEntity = await repository.InsertAsync(entity);

			// Act
			var fetchedEntity = await repository.GetAsync(new CategoryPrimaryKeyEntity { CategoryId = insertedEntity.CategoryId });

			// Assert
			Assert.Equal(insertedEntity.Name, fetchedEntity.Name);
			Assert.Equal(insertedEntity.Description, fetchedEntity.Description);
			Assert.Equal(insertedEntity.Picture, fetchedEntity.Picture);

			await repository.DeleteAsync(insertedEntity);
		}

		[Fact]
		public async Task Get_UseFullEntity_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};
			var insertedEntity = await repository.InsertAsync(entity);

			// Act
			var fetchedEntity = await repository.GetAsync(insertedEntity);

			// Assert
			Assert.Equal(insertedEntity.Description, fetchedEntity.Description);
			Assert.Equal(insertedEntity.Name, fetchedEntity.Name);
			Assert.Equal(insertedEntity.Picture, fetchedEntity.Picture);

			await repository.DeleteAsync(insertedEntity);
		}

		[Fact]
		public async Task Get_PrimaryKeyNotEntered_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentException>(async () => await repository.GetAsync(new CategoryPrimaryKeyEntity { }));
		}

		[Fact]
		public async Task Get_UseMissingPrimaryKey_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			await Assert.ThrowsAsync<NoEntityFoundException>(async () => await repository.GetAsync(new CategoryPrimaryKeyEntity { CategoryId = int.MaxValue }));
		}
		#endregion

		#region GetAll
		[Fact]
		public async Task GetAll_NoInput_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act
			var fetchedEntities = await repository.GetAllAsync();

			// Assert
			Assert.True(fetchedEntities.Count() > 0);
		}
		#endregion

		#region Insert
		[Fact]
		public async Task Insert_InputIsNull_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.InsertAsync(null!));
		}

		[Fact]
		public async Task Insert_HasIdentityKeyWithValue_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				CategoryId = 42,
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentException>(async () => await repository.InsertAsync(entity));
		}

		[Fact]
		public async Task Insert_HasIdentityKeyWithoutValue_IsInserted()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};

			// Act
			var insertedEntity = await repository.InsertAsync(entity);
			try
			{
				// Assert
				Assert.NotEqual(default, insertedEntity.CategoryId);
				Assert.Equal(entity.Description, insertedEntity.Description);
				Assert.Equal(entity.Name, insertedEntity.Name);
				Assert.Equal(entity.Picture, insertedEntity.Picture);
			}
			finally
			{
				await repository.DeleteAsync(insertedEntity);
			}
		}

		[Fact]
		public async Task Insert_NonNullPropertyMissing_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = null!,
				Picture = null
			};

			// Act && Assert
			await Assert.ThrowsAsync<SqlException>(async () => await repository.InsertAsync(entity));
		}
		#endregion

		#region Update
		[Fact]
		public async Task Update_InputIsNull_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.UpdateAsync(null!));
		}

		[Fact]
		public async Task Update_UseEntity_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Hello world"
			};
			var insertedEntity = await repository.InsertAsync(entity);

			var update = insertedEntity with { Description = "Something else" };

			// Act
			var updatedEntity = await repository.UpdateAsync(update);

			// Assert
			Assert.Equal("Something else", updatedEntity.Description);

			await repository.DeleteAsync(insertedEntity);
		}

		[Fact]
		public async Task Update_PrimaryKeyNotEntered_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Hello world"
			};

			// Act && Assert
			await Assert.ThrowsAsync<ArgumentException>(async () => await repository.UpdateAsync(entity));
		}

		[Fact]
		public async Task Update_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				CategoryId = int.MaxValue,
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Hello world"
			};

			// Act && Assert
			await Assert.ThrowsAsync<NoEntityFoundException>(async () => await repository.UpdateAsync(entity));
		}
		#endregion
	}
}
