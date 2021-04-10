namespace Ckode.Dapper.Repository.Tests
{
	public record Heap(

		string Username,
		string Password

		) : BaseTableRecord
	{
		public override string TableName => "Heaps";
	}
}
