using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entitys
{
	public record Heap : TableEntity
	{
		[Column]
		public string Username { get; init; }

		[Column]
		public string Password { get; init; }
	}
}
