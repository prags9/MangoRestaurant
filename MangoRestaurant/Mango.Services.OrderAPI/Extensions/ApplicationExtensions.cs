using Mango.Services.OrderAPI.Messaging;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Mango.Services.OrderAPI.Extensions
{
    public static class ApplicationExtensions
    {
        public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var host = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            host.ApplicationStarted.Register(OnStart);
            host.ApplicationStopped.Register(OnStop);
            return app;
        }
        private static void OnStart()
        {
            ServiceBusConsumer.Start();
        }
        private static void OnStop()
        {
            ServiceBusConsumer?.Stop();
        }
    }
}
