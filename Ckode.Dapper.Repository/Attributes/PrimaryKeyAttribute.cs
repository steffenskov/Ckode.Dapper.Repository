using System;

namespace Ckode.Dapper.Repository.Attributes
{
	/// <summary>
	/// Marks a property as a primary key.
	/// </summary>
	public class PrimaryKeyAttribute : Attribute
	{
		/// <summary>
		/// Whether this primary key is an idrecord column (auto incrementing value)
		/// </summary>
		public bool IsIdrecord { get; init; }

		/// <summary>
		/// </summary>
		/// <param name="isIdrecord">Whether this primary key is an idrecord column (auto incrementing value)</param>
		public PrimaryKeyAttribute(bool isIdrecord = false)
		{
			IsIdrecord = IsIdrecord;
		}
	}
}
