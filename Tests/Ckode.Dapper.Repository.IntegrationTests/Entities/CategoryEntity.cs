using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.IntegrationTests.Entities
{
	public record CategoryPrimaryKeyEntity : TableEntity
	{
		[PrimaryKeyColumn(true, "CategoryID")]
		public int CategoryId { get; init; }
	}

	public record CategoryEntity : CategoryPrimaryKeyEntity
	{
		[Column("CategoryName")]
		public string Name { get; init; } = default!;

		[Column]
		public string? Description { get; init; }

		[Column]
		public byte[]? Picture { get; init; }
	}
}
