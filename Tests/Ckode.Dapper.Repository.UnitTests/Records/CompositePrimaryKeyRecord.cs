using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Records
{
	internal record CompositePrimaryKeyRecord : TableRecord
	{
		[PrimaryKeyColumn]
		public string Username { get; init; }

		[PrimaryKeyColumn]
		public string Password { get; init; }

		[Column]
		public DateTime DateCreated { get; init; }
	}
}
