using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.IntegrationTests.Entities
{
	public record CompositeUserPrimaryKeyEntity : TableEntity
	{
		[PrimaryKeyColumn(isIdentity: false)]
		public string Username { get; init; }

		[PrimaryKeyColumn(isIdentity: false)]
		public string Password { get; init; }
	}

	public record CompositeUserEntity : CompositeUserPrimaryKeyEntity
	{
		[Column(hasDefaultConstraint: true)]
		public DateTime DateCreated { get; } // No init; as I want this value to never be set by the user

		[Column]
		public int? Age { get; init; }
	}
}
