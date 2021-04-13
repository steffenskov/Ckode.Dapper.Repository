using System;
using Ckode.Dapper.Repository.Tests.Records;
using Xunit;

namespace Ckode.Dapper.Repository.Tests
{
	public class SQLGeneratorTests
	{
		#region Constructor
		[Fact]
		public void Constructor_InputIsNull_Throws()
		{
			// Arrange, act && assert
			Assert.Throws<ArgumentNullException>(() => new SQLGenerator(null!));
		}

		[Fact]
		public void Constructor_InputIsWhitespace_Throws()
		{
			// Arrange, act && assert
			Assert.Throws<ArgumentException>(() => new SQLGenerator(" "));
		}
		#endregion

		#region Delete
		[Fact]
		public void GenerateDeleteQuery_InputIsNull_Throws()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			SinglePrimaryKeyRecord? record = null;

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => generator.GenerateDeleteQuery(record!));
		}

		[Fact]
		public void GenerateDeleteQuery_OnePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new SinglePrimaryKeyRecord { Id = 42, Username = "My name", Password = "My password" };

			// Act
			var deleteQuery = generator.GenerateDeleteQuery(record);

			// Assert
			Assert.Equal($"DELETE FROM Users OUTPUT deleted.* WHERE Id = @Id", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new CompositePrimaryKeyRecord { Username = "My name", Password = "My password", DateCreated = DateTime.Now };

			// Act
			var deleteQuery = generator.GenerateDeleteQuery(record);

			// Assert
			Assert.Equal($"DELETE FROM Users OUTPUT deleted.* WHERE Username = @Username AND Password = @Password", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new CustomColumnNamesRecord { Id = 42 };

			// Act
			var deleteQuery = generator.GenerateDeleteQuery(record);

			// Assert
			Assert.Equal($"DELETE FROM Users OUTPUT deleted.* WHERE OrderId = @Id", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_NoPrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new Heap { Username = "My name", Password = "My password" };

			// Act
			var deleteQuery = generator.GenerateDeleteQuery(record);

			// Assert
			Assert.Equal($"DELETE FROM Users OUTPUT deleted.* WHERE Username = @Username AND Password = @Password", deleteQuery);
		}
		#endregion

		#region Insert
		[Fact]
		public void GenerateInsertQuery_InputIsNull_Throws()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			SinglePrimaryKeyRecord record = null!;

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => generator.GenerateInsertQuery(record));
		}

		[Fact]
		public void GenerateInsertQuery_IdentityValuePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new SinglePrimaryKeyRecord { Username = "My name", Password = "My password" };

			// Act
			var insertQuery = generator.GenerateInsertQuery(record);

			// Assert
			Assert.Equal($"INSERT INTO Users (Username, Password) OUTPUT inserted.* VALUES (@Username, @Password)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_MissingColumnValue_ContainsColumn()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new CompositePrimaryKeyRecord { Username = "My name", Password = "My password" };

			// Act
			var insertQuery = generator.GenerateInsertQuery(record);

			// Assert
			Assert.Equal($"INSERT INTO Users (Username, Password, DateCreated) OUTPUT inserted.* VALUES (@Username, @Password, @DateCreated)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new CompositePrimaryKeyRecord { Username = "My name", Password = "My password", DateCreated = DateTime.Now };

			// Act
			var insertQuery = generator.GenerateInsertQuery(record);

			// Assert
			Assert.Equal($"INSERT INTO Users (Username, Password, DateCreated) OUTPUT inserted.* VALUES (@Username, @Password, @DateCreated)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new CustomColumnNamesRecord { Date = DateTime.Now };

			// Act
			var insertQuery = generator.GenerateInsertQuery(record);

			// Assert
			Assert.Equal($"INSERT INTO Users (DateCreated) OUTPUT inserted.* VALUES (@Date)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_NoPrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new Heap { Username = "My name", Password = "My password" };

			// Act
			var insertQuery = generator.GenerateInsertQuery(record);

			// Assert
			Assert.Equal($"INSERT INTO Users (Username, Password) OUTPUT inserted.* VALUES (@Username, @Password)", insertQuery);
		}
		#endregion

		#region GetAll
		[Fact]
		public void GenerateGetAllQuery_ProperTableName_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var selectQuery = generator.GenerateGetAllQuery();

			// Assert
			Assert.Equal($"SELECT * FROM Users", selectQuery);
		}
		#endregion

		#region Get
		[Fact]
		public void GenerateGetQuery_InputIsNull_Throws()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			Heap record = null!;

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => generator.GenerateGetQuery(record));
		}

		[Fact]
		public void GenerateGetQuery_SinglePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new SinglePrimaryKeyRecord { Id = 42 };

			// Act
			var selectQuery = generator.GenerateGetQuery(record);

			// Assert
			Assert.Equal($"SELECT * FROM Users WHERE Id = @Id", selectQuery);
		}

		[Fact]
		public void GenerateGetQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new CompositePrimaryKeyRecord { Username = "Hello world", Password = "My Password" };

			// Act
			var selectQuery = generator.GenerateGetQuery(record);

			// Assert
			Assert.Equal($"SELECT * FROM Users WHERE Username = @Username AND Password = @Password", selectQuery);
		}

		[Fact]
		public void GenerateGetQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new CustomColumnNamesRecord { Id = 42 };

			// Act
			var selectQuery = generator.GenerateGetQuery(record);

			// Assert
			Assert.Equal($"SELECT * FROM Users WHERE OrderId = @Id", selectQuery);
		}

		[Fact]
		public void GenerateGetQuery_NoPrimaryKey_Throws()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new Heap { Username = "Hello world", Password = "My Password" };

			// Act && Assert
			Assert.Throws<InvalidOperationException>(() => generator.GenerateGetQuery(record));
		}
		#endregion

		#region Update
		[Fact]
		public void GenerateUpdateQuery_InputIsNull_Throws()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			SinglePrimaryKeyRecord record = null!;

			// Act && Assert
			Assert.Throws<ArgumentNullException>(() => generator.GenerateUpdateQuery(record));
		}

		[Fact]
		public void GenerateUpdateQuery_SinglePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new SinglePrimaryKeyRecord { Id = 42 };

			// Act 
			var updateQuery = generator.GenerateUpdateQuery(record);

			// Assert
			Assert.Equal($"UPDATE Users SET Username = @Username, Password = @Password OUTPUT inserted.* WHERE Id = @Id", updateQuery);
		}

		[Fact]
		public void GenerateUpdateQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new CompositePrimaryKeyRecord { Username = "Hello world", Password = "My password" };

			// Act 
			var updateQuery = generator.GenerateUpdateQuery(record);

			// Assert
			Assert.Equal($"UPDATE Users SET DateCreated = @DateCreated OUTPUT inserted.* WHERE Username = @Username AND Password = @Password", updateQuery);
		}

		[Fact]
		public void GenerateUpdateQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new CustomColumnNamesRecord { Id = 42 };

			// Act 
			var updateQuery = generator.GenerateUpdateQuery(record);

			// Assert
			Assert.Equal($"UPDATE Users SET DateCreated = @Date OUTPUT inserted.* WHERE OrderId = @Id", updateQuery);
		}

		[Fact]
		public void GenerateUpdateQuery_NoPrimaryKey_Throws()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new Heap { Username = "Hello world", Password = "My password" };

			// Act && Assert
			Assert.Throws<InvalidOperationException>(() => generator.GenerateUpdateQuery(record));
		}
		#endregion
	}
}
