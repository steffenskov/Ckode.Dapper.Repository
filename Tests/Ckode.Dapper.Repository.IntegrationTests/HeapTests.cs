using System;
using System.Data.SqlClient;
using System.Linq;
using Ckode.Dapper.Repository.IntegrationTests.Entitys;
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
			var record = new UserHeapEntity { Username = "My name" };

			// Act && Assert
			Assert.Throws<NoEntityFoundException>(() => repository.Delete(record));
		}

		[Fact]
		public void Delete_HasAllColumns_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var record = new UserHeapEntity { Username = "My name1", Password = "My secret" };
			repository.Insert(record);

			// Act
			var deletedEntity = repository.Delete(record);

			// Assert
			Assert.Equal(record.Username, deletedEntity.Username);
			Assert.Equal(record.Password, deletedEntity.Password);
		}

		[Fact]
		public void Delete_MultipleRowsWithSameValues_DeletesBoth()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var record = new UserHeapEntity { Username = "My name1", Password = "My secret" };
			repository.Insert(record);
			repository.Insert(record);

			// Act
			var deletedEntity = repository.Delete(record);

			// Assert
			Assert.Throws<NoEntityFoundException>(() => repository.Get(record));
		}

		[Fact]
		public void Get_ValuesNotInDatabase_Throws()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var record = new UserHeapEntity { Username = "Not found", Password = "My secret" };

			// Act && Assert
			Assert.Throws<NoEntityFoundException>(() => repository.Get(record));
		}

		[Fact]
		public void Get_MultipleRowsWithSameValues_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var record = new UserHeapEntity { Username = "My name4", Password = "My secret" };
			repository.Insert(record);
			repository.Insert(record);

			// Act
			var gotten = repository.Get(record);

			try
			{
				// Assert
				Assert.Equal(record.Username, gotten.Username);
				Assert.Equal(record.Password, gotten.Password);
			}
			finally
			{
				repository.Delete(record);
			}
		}

		[Fact]
		public void GetAll_HaveRows_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var record = new UserHeapEntity { Username = "My name1", Password = "My secret" };
			repository.Insert(record);

			// Act
			var deletedEntity = repository.Delete(record);

			// Assert
			Assert.Equal(record.Username, deletedEntity.Username);
			Assert.Equal(record.Password, deletedEntity.Password);
		}

		[Fact]
		public void Insert_MissingColumns_Throws()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var record = new UserHeapEntity { Username = "My name" };

			// Act && Assert
			Assert.Throws<SqlException>(() => repository.Insert(record));
		}

		[Fact]
		public void Insert_HasAllColumns_Valid()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var record = new UserHeapEntity { Username = "My name2", Password = "My secret" };

			// Act
			var insertedEntity = repository.Insert(record);
			try
			{
				// Assert
				Assert.Equal(record.Username, insertedEntity.Username);
				Assert.Equal(record.Password, insertedEntity.Password);
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
			var record = new UserHeapEntity { Username = "My name3", Password = "My secret" };

			// Act
			repository.Insert(record);
			repository.Insert(record);
			try
			{
				// Assert
				var count = repository.GetAll().Count();
				Assert.True(count > 1);
			}
			finally
			{
				repository.Delete(record);
			}
		}
	}
}