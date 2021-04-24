using System;
public class NoEntityFoundException : Exception
{
	public string Query { get; init; }
	public string PrimaryKeys { get; init; }

	public NoEntityFoundException(string query, string primaryKeys, string message) : base(message)
	{
		this.Query = query;
		this.PrimaryKeys = primaryKeys;
	}
}