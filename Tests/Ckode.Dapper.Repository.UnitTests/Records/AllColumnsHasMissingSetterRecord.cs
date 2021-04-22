using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Records
{
	public record AllColumnsHasMissingSetterRecord : TableRecord
	{
		[PrimaryKeyColumn]
		public int Id { get; init; }

		[Column(hasDefaultConstraint: true)]
		public DateTime DateCreated { get; }
	}
}