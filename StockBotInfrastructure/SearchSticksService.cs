using Bybit.Net.Enums;
using StockBotInfrastructure.Models;

namespace StockBotInfrastructure;

public class SearchSticksService
{
    public async Task Searcher()
    {
        var clientConfig = new ClientConfiguration("IHl49Ahviqrfpav2h9", "xtBb713kO2skiaaVEOupPAtI10TDGktY0VwF");
        var client = new SetupExchangeClient(clientConfig).BybitRestClient;
        var sticks = await client.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT", KlineInterval.OneMinute);
    }
}