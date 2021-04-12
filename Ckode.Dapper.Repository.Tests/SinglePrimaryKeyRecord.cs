using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests
{
	public record SinglePrimaryKeyRecord : BaseTableRecord
	{
		public SinglePrimaryKeyRecord()
		{
		}

		[PrimaryKey(IsIdrecord = true)]
		[Column]
		public int Id { get; init; }

		[Column]
		public string Username { get; init; }

		[Column]
		public string Password { get; init; }

		public override string TableName => "Users";
	}
}
