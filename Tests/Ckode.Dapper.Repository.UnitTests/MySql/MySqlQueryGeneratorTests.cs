using System;
using Ckode.Dapper.Repository.MySql;
using Ckode.Dapper.Repository.Tests.Entities;
using Xunit;

namespace Ckode.Dapper.Repository.UnitTests.MySql
{
	public class QueryGeneratorTests
	{
		#region Constructor

		[Fact]
		public void Constructor_TableNameIsNull_Throws()
		{
			// Arrange, Act && Assert
			Assert.Throws<ArgumentNullException>(() => new MySqlQueryGenerator(null!));
		}


		[Fact]
		public void Constructor_TableNameIsWhiteSpace_Throws()
		{
			// Arrange, Act && Assert
			Assert.Throws<ArgumentException>(() => new MySqlQueryGenerator(" "));
		}
		#endregion

		#region Delete

		[Fact]
		public void GenerateDeleteQuery_OnePrimaryKey_Valid()
		{
			// Arrange
			var generator = new MySqlQueryGenerator("Users");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<SinglePrimaryKeyEntity>();

			// Assert
			Assert.Equal($@"SELECT Users.Id, Users.Username, Users.Password FROM Users WHERE Users.Id = @Id;
DELETE FROM Users WHERE Users.Id = @Id;", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_CompositePrimaryKey_Valid()
		{
			// Arrange
			var generator = new MySqlQueryGenerator("Users");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<CompositePrimaryKeyEntity>();

			// Assert
			Assert.Equal($@"SELECT Users.Username, Users.Password, Users.DateCreated FROM Users WHERE Users.Username = @Username AND Users.Password = @Password;
DELETE FROM Users WHERE Users.Username = @Username AND Users.Password = @Password;", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new MySqlQueryGenerator("Orders");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<CustomColumnNamesEntity>();

			// Assert
			Assert.Equal($@"SELECT Orders.OrderId AS Id, Orders.DateCreated AS Date FROM Orders WHERE Orders.OrderId = @Id;
DELETE FROM Orders WHERE Orders.OrderId = @Id;", deleteQuery);
		}

		[Fact]
		public void GenerateDeleteQuery_NoPrimaryKey_Valid()
		{
			// Arrange
			var generator = new MySqlQueryGenerator("Users");

			// Act
			var deleteQuery = generator.GenerateDeleteQuery<HeapEntity>();

			// Assert
			Assert.Equal($@"SELECT Users.Username, Users.Password FROM Users WHERE Users.Username = @Username AND Users.Password = @Password;
DELETE FROM Users WHERE Users.Username = @Username AND Users.Password = @Password;", deleteQuery);
		}
		#endregion

		#region GetAll
		[Fact]
		public void GenerateGetAllQuery_ProperTableName_Valid()
		{
			// Arrange
			var generator = new MySqlQueryGenerator("Users");

			// Act
			var selectQuery = generator.GenerateGetAllQuery<HeapEntity>();

			// Assert
			Assert.Equal($"SELECT Users.Username, Users.Password FROM Users;", selectQuery);
		}

		[Fact]
		public void GenerateGetAllQuery_CustomColumnNames_Valid()
		{
			// Arrange
			var generator = new MySqlQueryGenerator("Orders");

			// Act
			var selectQuery = generator.GenerateGetAllQuery<CustomColumnNamesEntity>();

			// Assert
			Assert.Equal($"SELECT Orders.OrderId AS Id, Orders.DateCreated AS Date FROM Orders;", selectQuery);
		}
		#endregion

	}
}
