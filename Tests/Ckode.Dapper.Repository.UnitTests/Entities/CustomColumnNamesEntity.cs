using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.UnitTests.Entities
{
	public record CustomColumnNamesEntity : DbEntity
	{
		[PrimaryKeyColumn(isIdentity: true, columnName: "OrderId")]
		public int Id { get; init; }

		[Column("DateCreated")]
		public DateTime Date { get; init; }
	}
}
