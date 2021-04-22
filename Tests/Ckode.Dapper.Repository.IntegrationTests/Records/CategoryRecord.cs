using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.IntegrationTests.Records
{
	public record CategoryPrimaryKeyRecord : TableRecord
	{
		[PrimaryKeyColumn(true, "CategoryID")]
		public int CategoryId { get; init; }
	}

	public record CategoryRecord : CategoryPrimaryKeyRecord
	{
		[Column("CategoryName")]
		public string Name { get; init; }

		[Column]
		public string? Description { get; init; }

		[Column]
		public byte[]? Picture { get; init; }
	}
}
