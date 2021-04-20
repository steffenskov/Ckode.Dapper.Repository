using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Records
{
    public record SinglePrimaryKeyRecord : TableRecord
    {
        public SinglePrimaryKeyRecord()
        {
        }

        [PrimaryKey(IsIdentity = true)]
        [Column]
        public int Id { get; init; }

        [Column]
        public string Username { get; init; }

        [Column]
        public string Password { get; init; }
    }
}
