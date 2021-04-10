using System;
using Xunit;

namespace Ckode.Dapper.Repository.Tests
{
	public class SQLGeneratorTests
	{
		[Fact]
		public void GenerateDeleteQuery_InputIsNull_Throws()
		{
			// Arrange
			var generator = new SQLGenerator();
			SinglePrimaryKeyRecord entity = null;

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => generator.GenerateDeleteQuery(entity));
		}

		[Fact]
		public void GenerateDeleteQuery_OnePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator();
			var entity = new SinglePrimaryKeyRecord { Id = 42, Username = "My name", Password = "My password" };

			// Act
			var deleteQuery = generator.GenerateDeleteQuery(entity);

			// Assert
			Assert.Equal($"DELETE FROM {entity.TableName} OUTPUT deleted.* WHERE Id = @Id", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator();
			var entity = new CompositePrimaryKeyRecord { Username = "My name", Password = "My password", DateCreated = DateTime.Now };

			// Act
			var deleteQuery = generator.GenerateDeleteQuery(entity);

			// Assert
			Assert.Equal($"DELETE FROM {entity.TableName} OUTPUT deleted.* WHERE Username = @Username AND Password = @Password", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_NoPrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator();
			var entity = new Heap("My name", "My password");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery(entity);

			// Assert
			Assert.Equal($"DELETE FROM {entity.TableName} OUTPUT deleted.* WHERE Username = @Username AND Password = @Password", deleteQuery);
		}


		[Fact]
		public void GenerateInsertQuery_InputIsNull_Throws()
		{
			// Arrange
			var generator = new SQLGenerator();
			SinglePrimaryKeyRecord entity = null;

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => generator.GenerateInsertQuery(entity));
		}

		[Fact]
		public void GenerateInsertQuery_IdentityValuePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator();
			var entity = new SinglePrimaryKeyRecord { Username = "My name", Password = "My password" };

			// Act
			var InsertQuery = generator.GenerateInsertQuery(entity);

			// Assert
			Assert.Equal($"INSERT INTO {entity.TableName} (Username, Password) OUTPUT inserted.* VALUES (@Username, @Password)", InsertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_IdentityReferencePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator();
			var entity = new CompositePrimaryKeyRecord { Password = "My password", DateCreated = DateTime.Now };

			// Act
			var InsertQuery = generator.GenerateInsertQuery(entity);

			// Assert
			Assert.Equal($"INSERT INTO {entity.TableName} (Password, DateCreated) OUTPUT inserted.* VALUES (@Password, @DateCreated)", InsertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_MissingColumnValue_ContainsColumn()
		{
			// Arrange
			var generator = new SQLGenerator();
			var entity = new CompositePrimaryKeyRecord { Username = "My name", Password = "My password" };

			// Act
			var InsertQuery = generator.GenerateInsertQuery(entity);

			// Assert
			Assert.Equal($"INSERT INTO {entity.TableName} (Username, Password, DateCreated) OUTPUT inserted.* VALUES (@Username, @Password, @DateCreated)", InsertQuery);
		}


		[Fact]
		public void GenerateInsertQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator();
			var entity = new CompositePrimaryKeyRecord { Username = "My name", Password = "My password", DateCreated = DateTime.Now };

			// Act
			var InsertQuery = generator.GenerateInsertQuery(entity);

			// Assert
			Assert.Equal($"INSERT INTO {entity.TableName} (Username, Password, DateCreated) OUTPUT inserted.* VALUES (@Username, @Password, @DateCreated)", InsertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_NoPrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator();
			var entity = new Heap("My name", "My password");

			// Act
			var InsertQuery = generator.GenerateInsertQuery(entity);

			// Assert
			Assert.Equal($"INSERT INTO {entity.TableName} (Username, Password) OUTPUT inserted.* VALUES (@Username, @Password)", InsertQuery);
		}
	}
}
