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
		public void GenerateDeleteQuery_OnePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<SinglePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"DELETE FROM [Users] OUTPUT [deleted].[Id], [deleted].[Username], [deleted].[Password] WHERE [Users].[Id] = @Id", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<CompositePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"DELETE FROM [Users] OUTPUT [deleted].[Username], [deleted].[Password], [deleted].[DateCreated] WHERE [Users].[Username] = @Username AND [Users].[Password] = @Password", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<CustomColumnNamesRecord>();

			// Assert
			Assert.Equal($"DELETE FROM [Orders] OUTPUT [deleted].[OrderId] AS [Id], [deleted].[DateCreated] AS [Date] WHERE [Orders].[OrderId] = @Id", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_NoPrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<Heap>();

			// Assert
			Assert.Equal($"DELETE FROM [Users] OUTPUT [deleted].[Username], [deleted].[Password] WHERE [Users].[Username] = @Username AND [Users].[Password] = @Password", deleteQuery);
		}
		#endregion

		#region Insert
		[Fact]
		public void GenerateInsertQuery_IdentityValuePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var insertQuery = generator.GenerateInsertQuery<SinglePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"INSERT INTO [Users] ([Username], [Password]) OUTPUT [inserted].[Id], [inserted].[Username], [inserted].[Password] VALUES (@Username, @Password)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_MissingColumnValue_ContainsColumn()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var insertQuery = generator.GenerateInsertQuery<CompositePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"INSERT INTO [Users] ([Username], [Password], [DateCreated]) OUTPUT [inserted].[Username], [inserted].[Password], [inserted].[DateCreated] VALUES (@Username, @Password, @DateCreated)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var insertQuery = generator.GenerateInsertQuery<CompositePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"INSERT INTO [Users] ([Username], [Password], [DateCreated]) OUTPUT [inserted].[Username], [inserted].[Password], [inserted].[DateCreated] VALUES (@Username, @Password, @DateCreated)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act
			var insertQuery = generator.GenerateInsertQuery<CustomColumnNamesRecord>();

			// Assert
			Assert.Equal($"INSERT INTO [Orders] ([DateCreated]) OUTPUT [inserted].[OrderId] AS [Id], [inserted].[DateCreated] AS [Date] VALUES (@Date)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_NoPrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var insertQuery = generator.GenerateInsertQuery<Heap>();

			// Assert
			Assert.Equal($"INSERT INTO [Users] ([Username], [Password]) OUTPUT [inserted].[Username], [inserted].[Password] VALUES (@Username, @Password)", insertQuery);
		}
		#endregion

		#region GetAll
		[Fact]
		public void GenerateGetAllQuery_ProperTableName_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var selectQuery = generator.GenerateGetAllQuery<Heap>();

			// Assert
			Assert.Equal($"SELECT [Users].[Username], [Users].[Password] FROM [Users]", selectQuery);
		}

		[Fact]
		public void GenerateGetAllQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act
			var selectQuery = generator.GenerateGetAllQuery<CustomColumnNamesRecord>();

			// Assert
			Assert.Equal($"SELECT [Orders].[OrderId] AS [Id], [Orders].[DateCreated] AS [Date] FROM [Orders]", selectQuery);
		}
		#endregion

		#region Get
		[Fact]
		public void GenerateGetQuery_SinglePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var selectQuery = generator.GenerateGetQuery<SinglePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"SELECT [Users].[Id], [Users].[Username], [Users].[Password] FROM [Users] WHERE [Users].[Id] = @Id", selectQuery);
		}

		[Fact]
		public void GenerateGetQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var selectQuery = generator.GenerateGetQuery<CompositePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"SELECT [Users].[Username], [Users].[Password], [Users].[DateCreated] FROM [Users] WHERE [Users].[Username] = @Username AND [Users].[Password] = @Password", selectQuery);
		}

		[Fact]
		public void GenerateGetQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act
			var selectQuery = generator.GenerateGetQuery<CustomColumnNamesRecord>();

			// Assert
			Assert.Equal($"SELECT [Orders].[OrderId] AS [Id], [Orders].[DateCreated] AS [Date] FROM [Orders] WHERE [Orders].[OrderId] = @Id", selectQuery);
		}

		[Fact]
		public void GenerateGetQuery_NoPrimaryKey_Throws()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act && Assert
			Assert.Throws<InvalidOperationException>(() => generator.GenerateGetQuery<Heap>());
		}
		#endregion

		#region Update

		[Fact]
		public void GenerateUpdateQuery_SinglePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act 
			var updateQuery = generator.GenerateUpdateQuery<SinglePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"UPDATE [Users] SET [Users].[Username] = @Username, [Users].[Password] = @Password OUTPUT [inserted].[Id], [inserted].[Username], [inserted].[Password] WHERE [Users].[Id] = @Id", updateQuery);
		}

		[Fact]
		public void GenerateUpdateQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act 
			var updateQuery = generator.GenerateUpdateQuery<CompositePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"UPDATE [Users] SET [Users].[DateCreated] = @DateCreated OUTPUT [inserted].[Username], [inserted].[Password], [inserted].[DateCreated] WHERE [Users].[Username] = @Username AND [Users].[Password] = @Password", updateQuery);
		}

		[Fact]
		public void GenerateUpdateQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act 
			var updateQuery = generator.GenerateUpdateQuery<CustomColumnNamesRecord>();

			// Assert
			Assert.Equal($"UPDATE [Orders] SET [Orders].[DateCreated] = @Date OUTPUT [inserted].[OrderId] AS [Id], [inserted].[DateCreated] AS [Date] WHERE [Orders].[OrderId] = @Id", updateQuery);
		}

		[Fact]
		public void GenerateUpdateQuery_NoPrimaryKey_Throws()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act && Assert
			Assert.Throws<InvalidOperationException>(() => generator.GenerateUpdateQuery<Heap>());
		}
		#endregion
	}
}
