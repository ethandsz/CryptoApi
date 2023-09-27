using StockBotDomain.Models;
using StockBotInfrastructure;

namespace StockApi;

public class Startup
{
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        // Add services to the container.
        serviceCollection.AddControllers();
        serviceCollection.AddAuthorization();
        serviceCollection.AddSwaggerGen();
        ConfigureStickService(serviceCollection);
    }

    private void ConfigureStickService(IServiceCollection serviceCollection)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Startup>().Build();
        
        // Can currently only have one of these at a time, need to refactor SearchSticksService to allow for multiple
        var byBitBtcUsdt = config.GetSection("ByBitBTCUSDT").Get<ClientConfiguration>();
        var client = new SetupExchangeClient().SetupByBitClient(byBitBtcUsdt);
        var stickServiceBtcUsdt = new SearchSticksService(client);
        serviceCollection.AddSingleton<ISearchSticksService>(stickServiceBtcUsdt);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        
    }
}