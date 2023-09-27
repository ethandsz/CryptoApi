using Bybit.Net.Clients;
using Bybit.Net.Enums;

namespace StockBotInfrastructure;

public class SearchSticksService : ISearchSticksService
{
    public SearchSticksService(BybitRestClient bybitRestClient)
    { 
        BybitRestClient = bybitRestClient;
    }
    private BybitRestClient BybitRestClient { get; }
    
    public async Task Search()
    {
        while (true)
        {
            Console.WriteLine("Requesting sticks");
            var sticks = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
                KlineInterval.OneMinute);
        }
    }
}