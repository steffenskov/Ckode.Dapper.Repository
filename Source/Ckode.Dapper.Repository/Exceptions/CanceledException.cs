using System;

namespace Ckode.Dapper.Repository.Exceptions
{
	public class CanceledException : Exception
	{
		public CanceledException(string message) : base(message)
		{
		}
	}
}
