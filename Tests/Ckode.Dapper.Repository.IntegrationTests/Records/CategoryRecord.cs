using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.IntegrationTests.Records
{
	public record CategoryRecord : BaseTableRecord
	{
		[PrimaryKey(true)]
		[Column("CategoryID")]
		public int CategoryId { get; init; }

		[Column("CategoryName")]
		public string Name { get; init; }

		[Column]
		public string? Description { get; init; }

		[Column]
		public byte[]? Picture { get; init; }
	}
}
