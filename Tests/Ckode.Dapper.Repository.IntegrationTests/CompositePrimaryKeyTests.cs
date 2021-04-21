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
				Username = "My name",
				Password = "Secret"
			};
			var insertedRecord = repository.Insert(record);

			// Act
			var deleted = repository.Delete(new CompositeUserPrimaryKeyRecord { Username = "My name", Password = "Secret" });

			// Assert
			Assert.Equal(record.Username, deleted.Username);
			Assert.Equal(record.Password, deleted.Password);
			Assert.Equal(insertedRecord.DateCreated, deleted.DateCreated);
		}

		[Fact]
		public void Insert_RelyOnDefaultConstraint_Valid()
		{
			// Arrange
			var repository = new CompositeUserRepository();
			var record = new CompositeUserRecord
			{
				Username = "My name",
				Password = "Secret"
			};

			// Act
			var insertedRecord = repository.Insert(record);

			// Assert
			Assert.Equal(record.Username, insertedRecord.Username);
			Assert.Equal(record.Password, insertedRecord.Password);
			Assert.True(insertedRecord.DateCreated > DateTime.Now.AddHours(-1));
		}
	}
}