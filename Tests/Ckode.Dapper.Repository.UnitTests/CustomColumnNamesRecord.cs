using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests
{
	public record CustomColumnNamesRecord : BaseTableRecord
	{
		[PrimaryKey(IsIdrecord = true)]
		[Column("OrderId")]
		public int Id { get; init; }

		[Column("DateCreated")]
		public DateTime Date { get; init; }

		public override string TableName => "Orders";
	}
}
