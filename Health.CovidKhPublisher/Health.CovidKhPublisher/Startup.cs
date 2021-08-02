using Health.CovidKhPublisher.Business.Decorators;
using Health.CovidKhPublisher.Business.Interfaces;
using Health.CovidKhPublisher.Business.Models.Dto;
using Health.CovidKhPublisher.Business.Services;
using Health.CovidKhPublisher.Communication;
using Health.CovidKhPublisher.Communication.Interfaces;
using Health.CovidKhPublisher.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Health.CovidKhPublisher.Startup))]
namespace Health.CovidKhPublisher
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            services.AddOptions<AppSettings>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("AppSettings").Bind(settings);
            });

            services.AddHttpClient<IHttpCommunicationClient, HttpCommunicationClient>();
            services.AddSingleton<IMemoryCache, MemoryCache>();

            services.AddScoped<RnboReportProvider>();
            services.AddScoped<IRnboReportProvider>(ctx =>
                new RnboReportProviderCacheDecorator(ctx.GetService<RnboReportProvider>(), ctx.GetService<IMemoryCache>()));

            services.AddScoped<IMessagePublisher, TelegramMessagePublisher>();
            services.AddScoped<IMessageBuilder<AggregatedReportDto>, AggregatedReportMessageBuilder>();

            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IMessageService, MessageService>();
        }
    }
}
