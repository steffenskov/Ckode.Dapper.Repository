using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entitys
{
	public record ColumnHasMissingSetterEntity : TableEntity
	{
		[PrimaryKeyColumn]
		public int Id { get; init; }

		[Column]
		public int Age { get; init; }

		[Column(hasDefaultConstraint: true)]
		public DateTime DateCreated { get; }
	}
}