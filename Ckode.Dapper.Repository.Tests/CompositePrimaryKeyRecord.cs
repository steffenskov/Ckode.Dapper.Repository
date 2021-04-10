using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests
{
	internal record CompositePrimaryKeyRecord : BaseTableRecord
	{
		[PrimaryKey]
		public string Username { get; init; }

		[PrimaryKey]
		public string Password { get; init; }

		public DateTime DateCreated { get; init; }

		public override string TableName => "Users";
	}
}
