using System;
using Ckode.Dapper.Repository.Attributes;

namespace Ckode.Dapper.Repository.Tests.Records
{
    internal record CompositePrimaryKeyRecord : TableRecord
    {
        [PrimaryKey]
        [Column]
        public string Username { get; init; }

        [PrimaryKey]
        [Column]
        public string Password { get; init; }

        [Column]
        public DateTime DateCreated { get; init; }
    }
}
