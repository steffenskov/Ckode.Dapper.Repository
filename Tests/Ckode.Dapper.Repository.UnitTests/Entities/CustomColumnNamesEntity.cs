using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entities
{
	public record CustomColumnNamesEntity : DapperEntity
	{
		[PrimaryKeyColumn(isIdentity: true, columnName: "OrderId")]
		public int Id { get; init; }

		[Column("DateCreated")]
		public DateTime Date { get; init; }
	}
}
