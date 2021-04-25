using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Ckode.Dapper.Repository.IntegrationTests.Entities;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class HeapAsyncTests
	{
		[Fact]
		public async Task Delete_MissingColumns_Throws()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "My name" };

			// Act && Assert
			await Assert.ThrowsAsync<NoEntityFoundException>(async () => await repository.DeleteAsync(entity));
		}

		[Fact]
		public async Task Delete_HasAllColumns_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "My name1", Password = "My secret" };
			await repository.InsertAsync(entity);

			// Act
			var deletedEntity = await repository.DeleteAsync(entity);

			// Assert
			Assert.Equal(entity.Username, deletedEntity.Username);
			Assert.Equal(entity.Password, deletedEntity.Password);
		}

		[Fact]
		public async Task Delete_MultipleRowsWithSameValues_DeletesBoth()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "My name1", Password = "My secret" };
			await repository.InsertAsync(entity);
			await repository.InsertAsync(entity);

			// Act
			var deletedEntity = await repository.DeleteAsync(entity);

			// Assert
			await Assert.ThrowsAsync<NoEntityFoundException>(async () => await repository.GetAsync(entity));
		}

		[Fact]
		public async Task Get_ValuesNotInDatabase_Throws()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "Not found", Password = "My secret" };

			// Act && Assert
			await Assert.ThrowsAsync<NoEntityFoundException>(async () => await repository.GetAsync(entity));
		}

		[Fact]
		public async Task Get_MultipleRowsWithSameValues_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "My name4", Password = "My secret" };
			await repository.InsertAsync(entity);
			await repository.InsertAsync(entity);

			// Act
			var gotten = await repository.GetAsync(entity);

			try
			{
				// Assert
				Assert.Equal(entity.Username, gotten.Username);
				Assert.Equal(entity.Password, gotten.Password);
			}
			finally
			{
				await repository.DeleteAsync(entity);
			}
		}

		[Fact]
		public async Task GetAll_HaveRows_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "My name1", Password = "My secret" };
			await repository.InsertAsync(entity);

			// Act
			var deletedEntity = await repository.DeleteAsync(entity);

			// Assert
			Assert.Equal(entity.Username, deletedEntity.Username);
			Assert.Equal(entity.Password, deletedEntity.Password);
		}

		[Fact]
		public async Task Insert_MissingColumns_Throws()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "My name" };

			// Act && Assert
			await Assert.ThrowsAsync<SqlException>(async () => await repository.InsertAsync(entity));
		}

		[Fact]
		public async Task Insert_HasAllColumns_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "My name2", Password = "My secret" };

			// Act
			var insertedEntity = await repository.InsertAsync(entity);
			try
			{
				// Assert
				Assert.Equal(entity.Username, insertedEntity.Username);
				Assert.Equal(entity.Password, insertedEntity.Password);
			}
			finally
			{
				await repository.DeleteAsync(insertedEntity);
			}
		}

		[Fact]
		public async Task Insert_InsertSameValuesTwice_BothAreCreated()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "My name3", Password = "My secret" };

			// Act
			await repository.InsertAsync(entity);
			await repository.InsertAsync(entity);
			try
			{
				// Assert
				var count = (await repository.GetAllAsync()).Count();
				Assert.True(count > 1);
			}
			finally
			{
				await repository.DeleteAsync(entity);
			}
		}
	}
}