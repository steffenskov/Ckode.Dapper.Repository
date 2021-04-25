using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entities
{
	public record SinglePrimaryKeyEntity : TableEntity
	{
		public SinglePrimaryKeyEntity()
		{
		}

		[PrimaryKeyColumn(isIdentity: true)]
		public int Id { get; init; }

		[Column]
		public string Username { get; init; }

		[Column]
		public string Password { get; init; }
	}
}
