using System;
using System.Linq;
using Ckode.Dapper.Repository.Exceptions;
using Ckode.Dapper.Repository.IntegrationTests.Entities;
using Ckode.Dapper.Repository.IntegrationTests.Repositories;
using Xunit;

namespace Ckode.Dapper.Repository.IntegrationTests
{
	public class PrimaryKeyEventTests
	{

		#region Delete
		[Fact]
		public void Delete_PreInsertHasEvent_IsInvoked()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
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
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};
			var insertedEntity = repository.Insert(entity);

			repository.PreDelete += (inputEntity, cancelArgs) =>
			{
				throw new InvalidOperationException();
			};

			// Act
			var deletedEntity = repository.Delete(insertedEntity);

			// Assert
			Assert.True(deletedEntity.CategoryId > 0);
			Assert.Throws<NoEntityFoundException>(() => repository.Get(deletedEntity));
			Assert.Equal(deletedEntity, insertedEntity);
			Assert.NotSame(deletedEntity, insertedEntity); // Ensure we're not just handed the inputEntity back
		}

		[Fact]
		public void Delete_PreInsertCancels_IsCanceled()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
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
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};
			var insertedEntity = repository.Insert(entity);

			CategoryEntity deletedEntity = null!;
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
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};
			var insertedEntity = repository.Insert(entity);

			repository.PostDelete += (tmpEntity) =>
			{
				throw new InvalidOperationException();
			};

			// Act
			var result = repository.Delete(insertedEntity);

			// Assert
			Assert.True(result.CategoryId > 0);
			Assert.Throws<NoEntityFoundException>(() => repository.Get(result));
		}
		#endregion

		#region Insert
		[Fact]
		public void Insert_PreInsertHasEvent_IsInvoked()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
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
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
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
				Assert.True(insertedEntity.CategoryId > 0);
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
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};

			repository.PreInsert += (preInsertEntity, cancelArgs) =>
			{
				cancelArgs.Cancel = true;
			};

			// Act && Assert
			Assert.Throws<CanceledException>(() => repository.Insert(entity));

			var allNames = repository.GetAll().Select(category => category.Name);
			Assert.DoesNotContain(entity.Name, allNames);
		}

		[Fact]
		public void Insert_PostInsertHasEvent_IsInvoked()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};

			CategoryEntity postInsertEntity = null!;
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
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
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
				Assert.True(insertedEntity.CategoryId > 0);
			}
			finally
			{
				repository.Delete(insertedEntity);
			}
		}
		#endregion

		#region Update
		[Fact]
		public void Update_PreUpdateHasEvent_IsInvoked()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};

			var insertedEntity = repository.Insert(entity);

			var entityToUpdate = insertedEntity with { Description = "Hello world" };

			repository.PreUpdate += (preUpdateEntity, cancelArgs) =>
			{
				// Assert
				Assert.Equal(entityToUpdate, preUpdateEntity);
			};

			// Act
			repository.Update(entityToUpdate);

			repository.Delete(insertedEntity);
		}

		[Fact]
		public void Update_PreUpdateThrows_IsUpdated()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};
			var insertedEntity = repository.Insert(entity);

			repository.PreUpdate += (preUpdateEntity, cancelArgs) =>
			{
				throw new InvalidOperationException();
			};

			// Act
			var updatedEntity = repository.Update(insertedEntity with { Description = "Hello world" });

			// Assert
			try
			{
				Assert.Equal("Hello world", updatedEntity.Description);
			}
			finally
			{
				repository.Delete(insertedEntity);
			}
		}

		[Fact]
		public void Update_PreUpdateCancels_IsCancelled()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};

			repository.PreUpdate += (preUpdateEntity, cancelArgs) =>
			{
				cancelArgs.Cancel = true;
			};

			var insertedEntity = repository.Insert(entity);

			// Act && Assert
			Assert.Throws<CanceledException>(() => repository.Update(insertedEntity with { Description = "Hello world" }));
			var gottenEntity = repository.Get(insertedEntity);
			Assert.Equal(entity.Description, gottenEntity.Description);

			repository.Delete(insertedEntity);
		}

		[Fact]
		public void Update_PostUpdateHasEvent_IsInvoked()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};

			CategoryEntity postUpdateEntity = null!;
			repository.PostUpdate += (tmpEntity) =>
			{
				postUpdateEntity = tmpEntity;
			};

			// Act
			var insertedEntity = repository.Insert(entity);

			var updatedEntity = repository.Update(insertedEntity with { Description = "Hello world" });

			// Assert
			try
			{
				Assert.Equal(updatedEntity, postUpdateEntity);
				Assert.Same(updatedEntity, postUpdateEntity);
			}
			finally
			{
				repository.Delete(insertedEntity);
			}
		}

		[Fact]
		public void Update_PostUpdatedThrows_IsUpdated()
		{
			// Arrange
			var repository = new CategoryRepository();
			var entity = new CategoryEntity
			{
				Description = "Lorem ipsum, dolor sit amit",
				Name = GenerateName(),
				Picture = null
			};

			var insertedEntity = repository.Insert(entity);

			repository.PostUpdate += (tmpEntity) =>
			{
				throw new InvalidOperationException();
			};

			// Act
			var updatedEntity = repository.Update(insertedEntity with { Description = "Hello world" });

			// Assert
			try
			{
				Assert.Equal("Hello world", updatedEntity.Description);
			}
			finally
			{
				repository.Delete(insertedEntity);
			}
		}
		#endregion


		private string GenerateName()
		{
			return Guid.NewGuid().ToString().Remove(15);
		}
	}
}
