using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.IntegrationTests.Entities
{
	public record UserHeapEntity : TableEntity
	{
		[Column]
		public string Username { get; init; }

		[Column]
		public string Password { get; init; }
	}
}