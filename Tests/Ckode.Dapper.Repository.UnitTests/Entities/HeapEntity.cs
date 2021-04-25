using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entities
{
	public record HeapEntity : TableEntity
	{
		[Column]
		public string Username { get; init; }

		[Column]
		public string Password { get; init; }
	}
}
