using Bybit.Net.Clients;
using Bybit.Net.Enums;
using StockBotInfrastructure.Maps;

namespace StockBotInfrastructure;

public class SearchSticksService : ISearchSticksService
{
    public SearchSticksService(BybitRestClient bybitRestClient)
    {
        BybitRestClient = bybitRestClient;
        ByBitStickMapper = new ByBitStickMapper();
    }

    private BybitRestClient BybitRestClient { get; }
    private ByBitStickMapper ByBitStickMapper { get; }

    public async Task Search()
    {
        Console.WriteLine("Requesting sticks");
        var result = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
            KlineInterval.OneMinute);
        var sticks = result.Data.List.Select(kline => ByBitStickMapper.Map(kline));
    }
}