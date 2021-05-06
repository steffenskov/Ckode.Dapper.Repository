using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entities
{
	public record AllColumnsHasMissingSetterEntity : DbEntity
	{
		[PrimaryKeyColumn]
		public int Id { get; init; }

		[Column(hasDefaultConstraint: true)]
		public DateTime DateCreated { get; }
	}
}