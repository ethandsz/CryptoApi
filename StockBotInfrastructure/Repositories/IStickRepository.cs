using StockBotDomain.Models;

namespace StockBotInfrastructure.Repositories;

public interface IStickRepository
{
    public void AddBuyLevels(CandleStick[] candleSticks);
    public void AddSellLevels(CandleStick[] candleSticks);
    
    public CandleStick[] GetBuyLevels();
    public CandleStick[] GetSellLevels();
}