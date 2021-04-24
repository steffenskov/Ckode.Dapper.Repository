using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entitys
{
	public record CustomColumnNamesEntity : TableEntity
	{
		[PrimaryKeyColumn(isIdentity: true, columnName: "OrderId")]
		public int Id { get; init; }

		[Column("DateCreated")]
		public DateTime Date { get; init; }
	}
}
