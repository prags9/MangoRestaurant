using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mango.Messages
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        private string connectionString = "Endpoint=sb://man-go-re-set-au-r.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LWv3Tw/dmnwN86wU0FLYeckl2tKRHQCeI+ASbHY42Ao=";
        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topicName);
            var jsonMessage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finamMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };
            await sender.SendMessageAsync(finamMessage);
            await client.DisposeAsync();
        }
    }
}
