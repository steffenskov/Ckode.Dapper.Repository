using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests
{
	public record Heap : BaseTableRecord
	{
		[Column]
		public string Username { get; init; }

		[Column]
		public string Password { get; init; }


		public override string TableName => "Heaps";
	}
}
