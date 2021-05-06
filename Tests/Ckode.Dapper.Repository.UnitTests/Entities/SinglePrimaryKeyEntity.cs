using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entities
{
	public record SinglePrimaryKeyEntity : DbEntity
	{
		[PrimaryKeyColumn(isIdentity: true)]
		public int Id { get; init; }

		[Column]
		public string Username { get; init; } = default!;

		[Column]
		public string Password { get; init; } = default!;
	}
}
