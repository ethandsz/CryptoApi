using Bybit.Net.Clients;
using StockBotDomain.Models;

namespace StockBotInfrastructure;

public interface ISetupExchangeClient
{
    public BybitRestClient SetupByBitClient(ClientConfiguration clientConfiguration);

}