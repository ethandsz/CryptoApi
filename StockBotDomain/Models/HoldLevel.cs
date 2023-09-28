namespace StockBotDomain.Models;

public record HoldLevel
{
    public bool IsInverse { get; set; }
    public double Level { get; set; }
    public long TimeStamp { get; set; }
}