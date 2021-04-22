using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.IntegrationTests.Records
{
	public record UserHeapRecord : TableRecord
	{
		[Column]
		public string Username { get; init; }

		[Column]
		public string Password { get; init; }
	}
}