using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Records
{
	public record SinglePrimaryKeyRecord : TableRecord
	{
		public SinglePrimaryKeyRecord()
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
