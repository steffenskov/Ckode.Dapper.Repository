using System;
using Ckode.Dapper.Repository.IntegrationTests.Records;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class CompositePrimaryKeyTests
	{
		[Fact]
		public void Delete_PrimaryKeyPartiallyNotEntered_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			Assert.Throws<ArgumentException>(() => repository.Delete(new CompositeUserPrimaryKeyRecord { Username = "My name" }));
		}

		[Fact]
		public void Delete_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			Assert.Throws<NoRecordFoundException>(() => repository.Delete(new CompositeUserPrimaryKeyRecord { Username = "My name", Password = "Secret" }));
		}

		[Fact]
		public void Delete_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var record = new CompositeUserRecord
			{
				Username = "My name1",
				Password = "Secret"
			};
			var insertedRecord = repository.Insert(record);

			// Act
			var deleted = repository.Delete(new CompositeUserPrimaryKeyRecord { Username = "My name1", Password = "Secret" });

			// Assert
			Assert.Equal(record.Username, deleted.Username);
			Assert.Equal(record.Password, deleted.Password);
			Assert.Equal(insertedRecord.DateCreated, deleted.DateCreated);
		}

		[Fact]
		public void Get_PrimaryKeyPartiallyNotEntered_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			Assert.Throws<ArgumentException>(() => repository.Get(new CompositeUserPrimaryKeyRecord { Username = "My name" }));
		}

		[Fact]
		public void Get_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			Assert.Throws<NoRecordFoundException>(() => repository.Get(new CompositeUserPrimaryKeyRecord { Username = "My name", Password = "Secret" }));
		}

		[Fact]
		public void Get_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var record = new CompositeUserRecord
			{
				Username = "My name2",
				Password = "Secret"
			};
			var insertedRecord = repository.Insert(record);

			// Act
			var gotten = repository.Get(new CompositeUserPrimaryKeyRecord { Username = "My name2", Password = "Secret" });

			// Assert
			Assert.Equal(record.Username, gotten.Username);
			Assert.Equal(record.Password, gotten.Password);
			Assert.Equal(insertedRecord.DateCreated, gotten.DateCreated);

			repository.Delete(insertedRecord);
		}

		[Fact]
		public void Update_PrimaryKeyPartiallyNotEntered_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			Assert.Throws<ArgumentException>(() => repository.Update(new CompositeUserRecord { Username = "My name" }));
		}

		[Fact]
		public void Update_UseMissingPrimaryKeyValue_Throws()
		{
			// Arrange
			var repository = new CompositeUserRepository();

			// Act && Assert
			Assert.Throws<NoRecordFoundException>(() => repository.Update(new CompositeUserRecord { Username = "Doesnt exist", Password = "Secret" }));
		}

		[Fact]
		public void Update_UsePrimaryKey_Valid()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var record = new CompositeUserRecord
			{
				Username = "My name3",
				Password = "Secret"
			};
			var insertedRecord = repository.Insert(record);

			// Act
			var updated = repository.Update(insertedRecord with { Age = 42 });

			// Assert
			Assert.Equal(record.Username, updated.Username);
			Assert.Equal(record.Password, updated.Password);
			Assert.NotEqual(42, insertedRecord.Age);
			Assert.Equal(42, updated.Age);
			Assert.Equal(insertedRecord.DateCreated, updated.DateCreated);

			repository.Delete(insertedRecord);
		}
	}
}