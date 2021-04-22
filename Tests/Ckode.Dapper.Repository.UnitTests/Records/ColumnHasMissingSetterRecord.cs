using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Records
{
	public record ColumnHasMissingSetterRecord : TableRecord
	{
		[PrimaryKeyColumn]
		public int Id { get; init; }

		[Column]
		public int Age { get; init; }

		[Column(hasDefaultConstraint: true)]
		public DateTime DateCreated { get; }
	}
}