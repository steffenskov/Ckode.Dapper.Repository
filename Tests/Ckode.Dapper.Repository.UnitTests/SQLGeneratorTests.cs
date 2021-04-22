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
			Assert.Equal($"DELETE FROM [dbo].[Users] OUTPUT [deleted].[Id], [deleted].[Username], [deleted].[Password] WHERE [dbo].[Users].[Id] = @Id", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<CompositePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"DELETE FROM [dbo].[Users] OUTPUT [deleted].[Username], [deleted].[Password], [deleted].[DateCreated] WHERE [dbo].[Users].[Username] = @Username AND [dbo].[Users].[Password] = @Password", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<CustomColumnNamesRecord>();

			// Assert
			Assert.Equal($"DELETE FROM [dbo].[Orders] OUTPUT [deleted].[OrderId] AS [Id], [deleted].[DateCreated] AS [Date] WHERE [dbo].[Orders].[OrderId] = @Id", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_NoPrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<Heap>();

			// Assert
			Assert.Equal($"DELETE FROM [dbo].[Users] OUTPUT [deleted].[Username], [deleted].[Password] WHERE [dbo].[Users].[Username] = @Username AND [dbo].[Users].[Password] = @Password", deleteQuery);
		}
		#endregion

		#region Insert
		[Fact]
		public void GenerateInsertQuery_IdentityValuePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var insertQuery = generator.GenerateInsertQuery(new SinglePrimaryKeyRecord());

			// Assert
			Assert.Equal($"INSERT INTO [dbo].[Users] ([Username], [Password]) OUTPUT [inserted].[Id], [inserted].[Username], [inserted].[Password] VALUES (@Username, @Password)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_MissingColumnValue_ContainsColumn()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var insertQuery = generator.GenerateInsertQuery(new CompositePrimaryKeyRecord());

			// Assert
			Assert.Equal($"INSERT INTO [dbo].[Users] ([Username], [Password], [DateCreated]) OUTPUT [inserted].[Username], [inserted].[Password], [inserted].[DateCreated] VALUES (@Username, @Password, @DateCreated)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var insertQuery = generator.GenerateInsertQuery(new CompositePrimaryKeyRecord());

			// Assert
			Assert.Equal($"INSERT INTO [dbo].[Users] ([Username], [Password], [DateCreated]) OUTPUT [inserted].[Username], [inserted].[Password], [inserted].[DateCreated] VALUES (@Username, @Password, @DateCreated)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act
			var insertQuery = generator.GenerateInsertQuery(new CustomColumnNamesRecord());

			// Assert
			Assert.Equal($"INSERT INTO [dbo].[Orders] ([DateCreated]) OUTPUT [inserted].[OrderId] AS [Id], [inserted].[DateCreated] AS [Date] VALUES (@Date)", insertQuery);
		}

		[Fact]
		public void GenerateInsertQuery_NoPrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var insertQuery = generator.GenerateInsertQuery(new Heap());

			// Assert
			Assert.Equal($"INSERT INTO [dbo].[Users] ([Username], [Password]) OUTPUT [inserted].[Username], [inserted].[Password] VALUES (@Username, @Password)", insertQuery);
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
			Assert.Equal($"SELECT [dbo].[Users].[Username], [dbo].[Users].[Password] FROM [dbo].[Users]", selectQuery);
		}

		[Fact]
		public void GenerateGetAllQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act
			var selectQuery = generator.GenerateGetAllQuery<CustomColumnNamesRecord>();

			// Assert
			Assert.Equal($"SELECT [dbo].[Orders].[OrderId] AS [Id], [dbo].[Orders].[DateCreated] AS [Date] FROM [dbo].[Orders]", selectQuery);
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
			Assert.Equal($"SELECT [dbo].[Users].[Id], [dbo].[Users].[Username], [dbo].[Users].[Password] FROM [dbo].[Users] WHERE [dbo].[Users].[Id] = @Id", selectQuery);
		}

		[Fact]
		public void GenerateGetQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act
			var selectQuery = generator.GenerateGetQuery<CompositePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"SELECT [dbo].[Users].[Username], [dbo].[Users].[Password], [dbo].[Users].[DateCreated] FROM [dbo].[Users] WHERE [dbo].[Users].[Username] = @Username AND [dbo].[Users].[Password] = @Password", selectQuery);
		}

		[Fact]
		public void GenerateGetQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act
			var selectQuery = generator.GenerateGetQuery<CustomColumnNamesRecord>();

			// Assert
			Assert.Equal($"SELECT [dbo].[Orders].[OrderId] AS [Id], [dbo].[Orders].[DateCreated] AS [Date] FROM [dbo].[Orders] WHERE [dbo].[Orders].[OrderId] = @Id", selectQuery);
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
			Assert.Equal($"UPDATE [dbo].[Users] SET [dbo].[Users].[Username] = @Username, [dbo].[Users].[Password] = @Password OUTPUT [inserted].[Id], [inserted].[Username], [inserted].[Password] WHERE [dbo].[Users].[Id] = @Id", updateQuery);
		}

		[Fact]
		public void GenerateUpdateQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act 
			var updateQuery = generator.GenerateUpdateQuery<CompositePrimaryKeyRecord>();

			// Assert
			Assert.Equal($"UPDATE [dbo].[Users] SET [dbo].[Users].[DateCreated] = @DateCreated OUTPUT [inserted].[Username], [inserted].[Password], [inserted].[DateCreated] WHERE [dbo].[Users].[Username] = @Username AND [dbo].[Users].[Password] = @Password", updateQuery);
		}

		[Fact]
		public void GenerateUpdateQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Orders");

			// Act 
			var updateQuery = generator.GenerateUpdateQuery<CustomColumnNamesRecord>();

			// Assert
			Assert.Equal($"UPDATE [dbo].[Orders] SET [dbo].[Orders].[DateCreated] = @Date OUTPUT [inserted].[OrderId] AS [Id], [inserted].[DateCreated] AS [Date] WHERE [dbo].[Orders].[OrderId] = @Id", updateQuery);
		}

		[Fact]
		public void GenerateUpdateQuery_NoPrimaryKey_Throws()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Act && Assert
			Assert.Throws<InvalidOperationException>(() => generator.GenerateUpdateQuery<Heap>());
		}

		[Fact]
		public void GenerateDeleteQuery_CustomSchema_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users", "account");

			// Act
			var query = generator.GenerateDeleteQuery<SinglePrimaryKeyRecord>();

			// Assert
			Assert.Equal("DELETE FROM [account].[Users] OUTPUT [deleted].[Id], [deleted].[Username], [deleted].[Password] WHERE [account].[Users].[Id] = @Id", query);
		}

		[Fact]
		public void GenerateInsertQuery_CustomSchema_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users", "account");

			// Act
			var query = generator.GenerateInsertQuery(new SinglePrimaryKeyRecord());

			// Assert
			Assert.Equal("INSERT INTO [account].[Users] ([Username], [Password]) OUTPUT [inserted].[Id], [inserted].[Username], [inserted].[Password] VALUES (@Username, @Password)", query);
		}

		[Fact]
		public void GenerateInsertQuery_ColumnHasDefaultConstraintAndDefaultValue_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");

			// Actj
			var query = generator.GenerateInsertQuery(new HasDefaultConstraintRecord());

			// Assert
			Assert.Equal("INSERT INTO [dbo].[Users] ([Id]) OUTPUT [inserted].[Id], [inserted].[DateCreated] VALUES (@Id)", query);
		}

		[Fact]
		public void GenerateInsertQuery_ColumnHasDefaultConstraintAndNonDefaultValue_Valid()
		{
			// Arrange
			var generator = new SQLGenerator("Users");
			var record = new HasDefaultConstraintRecord
			{
				Id = 42,
				DateCreated = DateTime.Now
			};

			// Act
			var query = generator.GenerateInsertQuery(record);

			// Assert
			Assert.Equal("INSERT INTO [dbo].[Users] ([Id], [DateCreated]) OUTPUT [inserted].[Id], [inserted].[DateCreated] VALUES (@Id, @DateCreated)", query);
		}
		#endregion
	}
}
