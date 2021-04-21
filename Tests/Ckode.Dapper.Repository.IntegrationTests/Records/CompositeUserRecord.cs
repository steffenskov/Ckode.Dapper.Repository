using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.IntegrationTests.Records
{
	public record CompositeUserPrimaryKeyRecord : TableRecord
	{
		[PrimaryKey(isIdentity: false)]
		[Column]
		public string Username { get; init; }

		[PrimaryKey(isIdentity: false)]
		[Column]
		public string Password { get; init; }
	}

	public record CompositeUserRecord : CompositeUserPrimaryKeyRecord
	{
		[Column]
		public DateTime DateCreated { get; init; }
	}
}
