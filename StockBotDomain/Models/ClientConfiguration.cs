namespace StockBotDomain.Models;

public sealed record ClientConfiguration
{
    public string Symbol { get; init; } = null!;
    public string ApiSecret { get; init; } = null!;
    public string ApiKey { get; init; } = null!;
}