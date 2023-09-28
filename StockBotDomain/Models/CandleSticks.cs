namespace StockBotDomain.Models;

public record CandleSticks
{
    public long TimeStamp { get; set; }
    public string? Ticker { get; set; }
    public double Close { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Open { get; set; }
    public double Volume { get; set; }
    
    public bool IsBullish
    {
        get => Open < Close;
    }   
}