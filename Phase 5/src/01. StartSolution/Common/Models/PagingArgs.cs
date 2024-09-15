namespace Common.Models;

public class PagingArgs
{
	private const int DefaultLimit = 20;
	private const int DefaultOffset = 0;

	private int limit = DefaultLimit;

	public static readonly PagingArgs NoPaging = new() { UsePaging = false };
	public static readonly PagingArgs Default = new() { UsePaging = true, Limit = DefaultLimit, Offset = DefaultOffset };
	public static readonly PagingArgs FirstItem = new() { UsePaging = true, Limit = 1, Offset = DefaultOffset };

	public int Offset { get; set; } = DefaultOffset;

	public int Limit
	{
		get => this.limit;
		set => this.limit = value > 0 ? value : DefaultLimit;
	}

	public bool UsePaging { get; set; } = true;
}