using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Entities
{
	public record HasDefaultConstraintEntity : DbEntity
	{
		[PrimaryKeyColumn]
		public int Id { get; init; }

		[Column(hasDefaultConstraint: true)]
		public DateTime DateCreated { get; init; }
	}
}