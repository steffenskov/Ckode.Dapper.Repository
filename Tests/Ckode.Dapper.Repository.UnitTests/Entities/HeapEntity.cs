using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.UnitTests.Entities
{
	public record HeapEntity : DbEntity
	{
		[Column]
		public string Username { get; init; } = default!;

		[Column]
		public string Password { get; init; } = default!;
	}
}
