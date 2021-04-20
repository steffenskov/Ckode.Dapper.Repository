using System;
public class NoRecordFoundException : Exception
{
    public string Query { get; init; }
    public string PrimaryKeys { get; init; }

    public NoRecordFoundException(string query, string primaryKeys, string message) : base(message)
    {
        this.Query = query;
        this.PrimaryKeys = primaryKeys;
    }
}