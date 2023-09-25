using StockBotInfrastructure;

namespace StockApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        // Add services to the container.
        serviceCollection.AddControllers();
        serviceCollection.AddAuthorization();
        serviceCollection.AddSwaggerGen();
        ConfigureStickService();
    }

    private void ConfigureStickService()
    {
        var stickService = new SearchSticksService().Searcher();
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