using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entities
{
	internal record CompositePrimaryKeyEntity : TableEntity
	{
		[PrimaryKeyColumn]
		public string Username { get; init; } = default!;

		[PrimaryKeyColumn]
		public string Password { get; init; } = default!;

		[Column]
		public DateTime DateCreated { get; init; }
	}
}
