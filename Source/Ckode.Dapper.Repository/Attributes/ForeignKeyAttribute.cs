using System;

namespace Ckode.Dapper.Repository.Attributes
{
	/// <summary>
	/// Marks a property as a foreign key.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ForeignKeyAttribute : Attribute
	{
		public string? ReferencedPropertyName { get; init; }

		/// <summary>
		/// </summary>
		/// <param name="referencedPropertyName">Optional name of the property in the referenced class, if it doesn't match the property name.</param>
		public ForeignKeyAttribute(string? referencedPropertyName = null)
		{
			this.ReferencedPropertyName = referencedPropertyName;
		}
	}
}
