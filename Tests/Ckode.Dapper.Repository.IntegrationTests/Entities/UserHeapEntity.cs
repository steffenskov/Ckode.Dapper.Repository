using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.IntegrationTests.Entities
{
	public record UserHeapEntity : TableEntity
	{
		[Column]
		public string Username { get; init; } = default!;

		[Column]
		public string Password { get; init; } = default!;
	}
}