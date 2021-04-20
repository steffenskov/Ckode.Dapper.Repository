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
        public void Delete_UseMissingPrimaryKey_ReturnsNull()
        {
            // Arrange
            var repository = new CategoryRepository();

            // Act
            var deleted = repository.Delete(new CategoryPrimaryKeyRecord { CategoryId = int.MaxValue });

            // Assert
            Assert.Null(deleted);
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
        public void Get_UseMissingPrimaryKey_ReturnsNull()
        {
            // Arrange
            var repository = new CategoryRepository();

            // Act
            var fetchedRecord = repository.Get(new CategoryPrimaryKeyRecord { CategoryId = int.MaxValue });

            // Assert
            Assert.Null(fetchedRecord);
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
    }
}
