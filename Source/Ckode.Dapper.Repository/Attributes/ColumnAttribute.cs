using System;

namespace Ckode.Dapper.Repository.Attributes
{
	/// <summary>
	/// Marks a property for mapping with a DB column
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ColumnAttribute : Attribute
	{
		public string? ColumnName { get; init; }

		/// <summary>
		/// </summary>
		/// <param name="columnName">Optional name of the DB column, if it doesn't match the property name.</param>
		public ColumnAttribute(string? columnName = null)
		{
			ColumnName = columnName;
		}
	}
}
