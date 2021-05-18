using System;
using System.Linq;
using Ckode.Dapper.Repository.Exceptions;
using Ckode.Dapper.Repository.IntegrationTests.Entities;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class HeapEventTests
	{

		#region Delete
		[Fact]
		public void Delete_PreInsertHasEvent_IsInvoked()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};
			var insertedEntity = repository.Insert(entity);

			repository.PreDelete += (inputEntity, cancelArgs) =>
			{
				// Assert
				Assert.Equal(entity, inputEntity);
			};

			// Act
			repository.Delete(insertedEntity);
		}

		[Fact]
		public void Delete_PreInsertThrows_IsDeleted()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};
			var insertedEntity = repository.Insert(entity);

			repository.PreDelete += (inputEntity, cancelArgs) =>
			{
				throw new InvalidOperationException();
			};

			// Act
			var deletedEntity = repository.Delete(insertedEntity);

			// Assert
			Assert.Equal(insertedEntity, deletedEntity);
			Assert.Throws<NoEntityFoundException>(() => repository.Get(deletedEntity));
			Assert.NotSame(insertedEntity, deletedEntity); // Ensure we're not just handed the inputEntity back
		}

		[Fact]
		public void Delete_PreInsertCancels_IsCanceled()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};
			var insertedEntity = repository.Insert(entity);

			var shouldCancel = true;

			repository.PreDelete += (inputEntity, cancelArgs) =>
			{
				cancelArgs.Cancel = shouldCancel;
			};

			// Act && Assert
			Assert.Throws<CanceledException>(() => repository.Delete(insertedEntity));

			// Assert
			Assert.NotNull(repository.Get(insertedEntity));

			// Cleanup
			shouldCancel = false;
			repository.Delete(insertedEntity);
		}

		[Fact]
		public void Delete_PostDeleteHasEvent_IsInvoked()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};
			var insertedEntity = repository.Insert(entity);

			UserHeapEntity deletedEntity = null!;
			repository.PostDelete += (tmpEntity) =>
			{
				deletedEntity = tmpEntity;
			};

			// Act
			var result = repository.Delete(insertedEntity);

			// Assert
			Assert.Equal(result, deletedEntity);
			Assert.Same(result, deletedEntity);
		}

		[Fact]
		public void Delete_PostDeleteThrows_IsDeleted()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};
			var insertedEntity = repository.Insert(entity);

			repository.PostDelete += (tmpEntity) =>
			{
				throw new InvalidOperationException();
			};

			// Act
			var result = repository.Delete(insertedEntity);

			// Assert
			Assert.Equal(insertedEntity, result);
			Assert.Throws<NoEntityFoundException>(() => repository.Get(result));
		}
		#endregion

		#region Insert
		[Fact]
		public void Insert_PreInsertHasEvent_IsInvoked()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};

			repository.PreInsert += (preInsertEntity, cancelArgs) =>
			{
				// Assert
				Assert.Equal(entity, preInsertEntity);
			};

			// Act
			var insertedEntity = repository.Insert(entity);

			repository.Delete(insertedEntity);
		}

		[Fact]
		public void Insert_PreInsertThrows_IsInserted()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};

			repository.PreInsert += (preInsertEntity, cancelArgs) =>
			{
				throw new InvalidOperationException();
			};

			// Act
			var insertedEntity = repository.Insert(entity);

			// Assert
			try
			{
				Assert.Equal(insertedEntity, repository.Get(entity));
			}
			finally
			{
				repository.Delete(insertedEntity);
			}
		}

		[Fact]
		public void Insert_PreInsertCancels_IsCancelled()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};

			repository.PreInsert += (preInsertEntity, cancelArgs) =>
			{
				cancelArgs.Cancel = true;
			};

			// Act && Assert
			Assert.Throws<CanceledException>(() => repository.Insert(entity));

			Assert.Throws<NoEntityFoundException>(() => repository.Get(entity));
		}

		[Fact]
		public void Insert_PostInsertHasEvent_IsInvoked()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};

			UserHeapEntity postInsertEntity = null!;
			repository.PostInsert += (tmpEntity) =>
			{
				postInsertEntity = tmpEntity;
			};

			// Act
			var insertedEntity = repository.Insert(entity);

			// Assert
			try
			{
				Assert.Equal(insertedEntity, postInsertEntity);
				Assert.Same(insertedEntity, postInsertEntity);
			}
			finally
			{
				repository.Delete(insertedEntity);
			}
		}

		[Fact]
		public void Insert_PostInsertThrows_IsInserted()
		{
			// Arrange
			var repository = new UserHeapRepository();
			var entity = new UserHeapEntity
			{
				Password = GenerateName(),
				Username = GenerateName(),
			};

			repository.PostInsert += (tmpEntity) =>
			{
				throw new InvalidOperationException();
			};

			// Act
			var insertedEntity = repository.Insert(entity);

			// Assert
			try
			{
				Assert.Equal(insertedEntity, repository.Get(entity));
			}
			finally
			{
				repository.Delete(insertedEntity);
			}
		}
		#endregion

		private string GenerateName()
		{
			return Guid.NewGuid().ToString();
		}
	}
}
