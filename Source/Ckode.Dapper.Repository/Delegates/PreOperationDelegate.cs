using System.ComponentModel;

namespace Ckode.Dapper.Repository.Delegates
{
	public delegate void PreOperationDelegate<T>(T inputEntity, CancelEventArgs e);
}
