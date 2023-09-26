using Bybit.Net.Enums;
using StockBotDomain.Models;

namespace StockBotInfrastructure;

public class SearchSticksService : ISearchSticksService
{
    public SearchSticksService(ClientConfiguration clientConfiguration)
    {
        ClientConfiguration = clientConfiguration;
    }
    
    private ClientConfiguration ClientConfiguration { get; }
    
    public async Task Search()
    {
        var client = new SetupExchangeClient(ClientConfiguration).BybitRestClient;
        var sticks = await client.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, ClientConfiguration.Symbol,
                KlineInterval.OneMinute);
        
    }
}