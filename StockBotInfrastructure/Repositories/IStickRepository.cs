using StockBotDomain.Models;

namespace StockBotInfrastructure.Repositories;

public interface IStickRepository
{
    public void AddLevels(List<HoldLevel> candleSticks);
    public HoldLevel[] GetBuyLevels();
    public HoldLevel[] GetSellLevels();
}