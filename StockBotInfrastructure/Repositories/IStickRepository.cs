using StockBotDomain.Models;

namespace StockBotInfrastructure.Repositories;

public interface IStickRepository
{
    public void AddBuyLevels(CandleSticks[] candleSticks);
    public void AddSellLevels(CandleSticks[] candleSticks);
    
    public CandleSticks[] GetBuyLevels();
    public CandleSticks[] GetSellLevels();
}