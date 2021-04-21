using System;
using System.Data.SqlClient;
using System.Linq;
using Ckode.Dapper.Repository.IntegrationTests.Records;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class SinglePrimaryKeyTests
	{
		#region Delete
		[Fact]
		public void Delete_InputIsNull_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => repository.Delete(null!));
		}

		[Fact]
		public void Delete_PrimaryKeyNotEntered_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Hello world"
			};

			// Act && Assert
			Assert.Throws<ArgumentException>(() => repository.Delete(record));
		}

		[Fact]
		public void Delete_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			Assert.Throws<NoRecordFoundException>(() => repository.Delete(new CategoryPrimaryKeyRecord { CategoryId = int.MaxValue }));
		}

		[Fact]
		public void Delete_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};
			var insertedRecord = repository.Insert(record);

			// Act
			var deleted = repository.Delete(new CategoryPrimaryKeyRecord { CategoryId = insertedRecord.CategoryId });

			// Assert
			Assert.Equal(insertedRecord.CategoryId, deleted.CategoryId);
			Assert.Equal(record.Description, deleted.Description);
			Assert.Equal(record.Name, deleted.Name);
			Assert.Equal(record.Picture, deleted.Picture);
		}

		[Fact]
		public void Delete_UseRecord_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};
			var insertedRecord = repository.Insert(record);

			// Act
			var deleted = repository.Delete(insertedRecord);

			// Assert
			Assert.Equal(insertedRecord.CategoryId, deleted.CategoryId);
			Assert.Equal(record.Description, deleted.Description);
			Assert.Equal(record.Name, deleted.Name);
			Assert.Equal(record.Picture, deleted.Picture);
		}
		#endregion

		#region Get
		[Fact]
		public void Get_InputIsNull_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => repository.Get(null!));
		}

		[Fact]
		public void Get_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};
			var insertedRecord = repository.Insert(record);

			// Act
			var fetchedRecord = repository.Get(new CategoryPrimaryKeyRecord { CategoryId = insertedRecord.CategoryId });

			// Assert
			Assert.Equal(insertedRecord.Name, fetchedRecord.Name);
			Assert.Equal(insertedRecord.Description, fetchedRecord.Description);
			Assert.Equal(insertedRecord.Picture, fetchedRecord.Picture);

			repository.Delete(insertedRecord);
		}

		[Fact]
		public void Get_UseFullRecord_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Lorem ipsum",
				Picture = null
			};
			var insertedRecord = repository.Insert(record);

			// Act
			var fetchedRecord = repository.Get(insertedRecord);

			// Assert
			Assert.Equal(insertedRecord.Description, fetchedRecord.Description);
			Assert.Equal(insertedRecord.Name, fetchedRecord.Name);
			Assert.Equal(insertedRecord.Picture, fetchedRecord.Picture);

			repository.Delete(insertedRecord);
		}

		[Fact]
		public void Get_PrimaryKeyNotEntered_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			Assert.Throws<ArgumentException>(() => repository.Get(new CategoryPrimaryKeyRecord { }));
		}

		[Fact]
		public void Get_UseMissingPrimaryKey_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			Assert.Throws<NoRecordFoundException>(() => repository.Get(new CategoryPrimaryKeyRecord { CategoryId = int.MaxValue }));
		}
		#endregion

		#region GetAll
		[Fact]
		public void GetAll_NoInput_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act
			var fetchedRecords = repository.GetAll();

			// Assert
			Assert.True(fetchedRecords.Count() > 0);
		}
		#endregion

		#region Insert
		[Fact]
		public void Insert_InputIsNull_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => repository.Insert(null!));
		}

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
				Assert.Equal(record.Description, insertedRecord.Description);
				Assert.Equal(record.Name, insertedRecord.Name);
				Assert.Equal(record.Picture, insertedRecord.Picture);
			}
			finally
			{
				repository.Delete(insertedRecord);
			}
		}

		[Fact]
		public void Insert_NonNullPropertyMissing_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = null!,
				Picture = null
			};

			// Act && Assert
			Assert.Throws<SqlException>(() => repository.Insert(record));
		}
		#endregion

		#region Update
		[Fact]
		public void Update_InputIsNull_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => repository.Update(null!));
		}

		[Fact]
		public void Update_UseRecord_Valid()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Hello world"
			};
			var insertedRecord = repository.Insert(record);

			var update = insertedRecord with { Description = "Something else" };

			// Act
			var updatedRecord = repository.Update(update);

			// Assert
			Assert.Equal("Something else", updatedRecord.Description);

			repository.Delete(insertedRecord);
		}

		[Fact]
		public void Update_PrimaryKeyNotEntered_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Hello world"
			};

			// Act && Assert
			Assert.Throws<ArgumentException>(() => repository.Update(record));
		}

		[Fact]
		public void Update_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CategoryRepository();
			var record = new CategoryRecord
			{
				CategoryId = int.MaxValue,
				Description = "Lorem ipsum, dolor sit amit",
				Name = "Hello world"
			};

			// Act && Assert
			Assert.Throws<NoRecordFoundException>(() => repository.Update(record));
		}
		#endregion
	}
}
