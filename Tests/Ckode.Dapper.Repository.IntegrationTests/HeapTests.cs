using System;
using System.Data.SqlClient;
using System.Linq;
using Ckode.Dapper.Repository.IntegrationTests.Entities;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class HeapTests
	{
		[Fact]
		public void Delete_MissingColumns_Throws()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "async My name" };

			// Act && Assert
			Assert.Throws<NoEntityFoundException>(() => repository.Delete(entity));
		}

		[Fact]
		public void Delete_HasAllColumns_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "async My name1", Password = "My secret" };
			repository.Insert(entity);

			// Act
			var deletedEntity = repository.Delete(entity);

			// Assert
			Assert.Equal(entity.Username, deletedEntity.Username);
			Assert.Equal(entity.Password, deletedEntity.Password);
		}

		[Fact]
		public void Delete_MultipleRowsWithSameValues_DeletesBoth()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "async My name1", Password = "My secret" };
			repository.Insert(entity);
			repository.Insert(entity);

			// Act
			var deletedEntity = repository.Delete(entity);

			// Assert
			Assert.Throws<NoEntityFoundException>(() => repository.Get(entity));
		}

		[Fact]
		public void Get_ValuesNotInDatabase_Throws()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "Not found", Password = "My secret" };

			// Act && Assert
			Assert.Throws<NoEntityFoundException>(() => repository.Get(entity));
		}

		[Fact]
		public void Get_MultipleRowsWithSameValues_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "async My name4", Password = "My secret" };
			repository.Insert(entity);
			repository.Insert(entity);

			// Act
			var gotten = repository.Get(entity);

			try
			{
				// Assert
				Assert.Equal(entity.Username, gotten.Username);
				Assert.Equal(entity.Password, gotten.Password);
			}
			finally
			{
				repository.Delete(entity);
			}
		}

		[Fact]
		public void GetAll_HaveRows_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "Some name", Password = "My secret" };
			var entity2 = new UserHeapEntity { Username = "Some other name", Password = "My secret" };
			repository.Insert(entity);
			repository.Insert(entity2);

			// Act
			var all = repository.GetAll();

			try
			{
				Assert.True(all.Count() >= 2);
			}
			finally
			{
				repository.Delete(entity);
				repository.Delete(entity2);
			}
		}

		[Fact]
		public void Insert_MissingColumns_Throws()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "async My name" };

			// Act && Assert
			Assert.Throws<SqlException>(() => repository.Insert(entity));
		}

		[Fact]
		public void Insert_HasAllColumns_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "async My name2", Password = "My secret" };

			// Act
			var insertedEntity = repository.Insert(entity);
			try
			{
				// Assert
				Assert.Equal(entity.Username, insertedEntity.Username);
				Assert.Equal(entity.Password, insertedEntity.Password);
			}
			finally
			{
				repository.Delete(insertedEntity);
			}
		}

		[Fact]
		public void Insert_InsertSameValuesTwice_BothAreCreated()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity { Username = "async My name3", Password = "My secret" };

			// Act
			repository.Insert(entity);
			repository.Insert(entity);
			try
			{
				// Assert
				var count = repository.GetAll().Count();
				Assert.True(count > 1);
			}
			finally
			{
				repository.Delete(entity);
			}
		}
	}
}