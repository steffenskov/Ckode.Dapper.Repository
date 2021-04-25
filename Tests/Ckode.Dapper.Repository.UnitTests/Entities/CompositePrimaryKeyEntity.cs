using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entities
{
	internal record CompositePrimaryKeyEntity : TableEntity
	{
		[PrimaryKeyColumn]
		public string Username { get; init; }

		[PrimaryKeyColumn]
		public string Password { get; init; }

		[Column]
		public DateTime DateCreated { get; init; }
	}
}
