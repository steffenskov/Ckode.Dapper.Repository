using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests
{
	internal record CompositePrimaryKeyRecord : BaseTableRecord
	{
		[PrimaryKey]
		[Column]
		public string Username { get; init; }

		[PrimaryKey]
		[Column]
		public string Password { get; init; }

		[Column]
		public DateTime DateCreated { get; init; }

		public override string TableName => "Users";
	}
}
