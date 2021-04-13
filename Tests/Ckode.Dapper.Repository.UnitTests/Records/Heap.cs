using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Records
{
	public record Heap : BaseTableRecord
	{
		[Column]
		public string Username { get; init; }

		[Column]
		public string Password { get; init; }
	}
}
