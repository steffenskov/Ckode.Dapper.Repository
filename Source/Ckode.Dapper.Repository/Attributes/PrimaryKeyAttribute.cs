using System;

namespace Ckode.Dapper.Repository.Attributes
{
	/// <summary>
	/// Marks a property as a primary key.
	/// </summary>
	/// 
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class PrimaryKeyAttribute : Attribute
	{
		/// <summary>
		/// Whether this primary key is an identity column (auto incrementing value)
		/// </summary>
		public bool IsIdentity { get; init; }

		/// <summary>
		/// </summary>
		/// <param name="isIdentity">Whether this primary key is an identity column (auto incrementing value)</param>
		public PrimaryKeyAttribute(bool isIdentity = false)
		{
			IsIdentity = isIdentity;
		}
	}
}
