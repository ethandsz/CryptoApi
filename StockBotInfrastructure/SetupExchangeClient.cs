using System.Security.Authentication;
using Bybit.Net.Clients;
using CryptoExchange.Net.Authentication;
using StockBotDomain.Models;

namespace StockBotInfrastructure;

public class SetupExchangeClient : ISetupExchangeClient
{
    public BybitRestClient SetupByBitClient(ClientConfiguration clientConfiguration)
    {
        var client = new BybitRestClient(options =>
        {
            options.ApiCredentials = new ApiCredentials(clientConfiguration.ApiKey, clientConfiguration.ApiSecret);
        });
        EnsureByBitClientIsValid(client);
        return client;
    }

    private async void EnsureByBitClientIsValid(BybitRestClient client)
    {
        var result = await client.V5Api.Account.GetApiKeyInfoAsync();
        if (!result.Success)
        {
            throw new InvalidCredentialException("ByBit client is not valid, please check your api key and secret");
        }
    }
}
