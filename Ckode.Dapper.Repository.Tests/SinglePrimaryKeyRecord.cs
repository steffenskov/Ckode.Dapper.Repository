using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests
{
	internal record SinglePrimaryKeyRecord : BaseTableRecord
	{
		public SinglePrimaryKeyRecord()
		{
		}

		[PrimaryKey]
		public int Id { get; init; }
		public string Username { get; init; }
		public string Password { get; init; }

		public override string TableName => "Users";
	}
}
