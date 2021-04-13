using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ckode.Dapper.Repository
{
	/// <summary>
	/// Marker record to ensure only records are used
	/// </summary>
	public abstract record BaseTableRecord
	{
		public abstract string TableName { get; }
	}
}
