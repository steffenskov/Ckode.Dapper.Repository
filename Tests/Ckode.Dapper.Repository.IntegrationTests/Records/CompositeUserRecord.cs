using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.IntegrationTests.Records
{
	public record CompositeUserPrimaryKeyRecord : TableRecord
	{
		[PrimaryKeyColumn(isIdentity: false)]
		public string Username { get; init; }

		[PrimaryKeyColumn(isIdentity: false)]
		public string Password { get; init; }
	}

	public record CompositeUserRecord : CompositeUserPrimaryKeyRecord
	{
		[Column(hasDefaultConstraint: true)]
		public DateTime DateCreated { get; init; }
	}
}
