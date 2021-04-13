using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Records
{
	public record CustomColumnNamesRecord : BaseTableRecord
	{
		[PrimaryKey(IsIdentity = true)]
		[Column("OrderId")]
		public int Id { get; init; }

		[Column("DateCreated")]
		public DateTime Date { get; init; }
	}
}
