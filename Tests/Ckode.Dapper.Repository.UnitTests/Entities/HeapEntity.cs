using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entities
{
	public record HeapEntity : DapperEntity
	{
		[Column]
		public string Username { get; init; } = default!;

		[Column]
		public string Password { get; init; } = default!;
	}
}
