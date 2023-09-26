using Bybit.Net.Clients;
using CryptoExchange.Net.Authentication;
using StockBotDomain.Models;

namespace StockBotInfrastructure;

public class SetupExchangeClient
{
    public SetupExchangeClient(ClientConfiguration clientConfiguration)
    {
        var client = new BybitRestClient(options =>
        {
            options.ApiCredentials = new ApiCredentials(clientConfiguration.ApiKey, clientConfiguration.ApiSecret);
        });
        
        BybitRestClient = client;
    }
    public BybitRestClient BybitRestClient { get; }
}

