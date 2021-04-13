using System;

namespace Ckode.Dapper.Repository.Attributes
{
	/// <summary>
	/// Marks a property as a foreign key.
	/// </summary>
	public class ForeignKeyAttribute : Attribute
	{
		public string ReferencedColumnName { get; init; }

		/// <summary>
		/// </summary>
		/// <param name="referencedColumnName">Optional name of the column in the referenced table, if it doesn't match the property name.</param>
		public ForeignKeyAttribute(string referencedColumnName = null)
		{
			this.ReferencedColumnName = referencedColumnName;
		}
	}
}
