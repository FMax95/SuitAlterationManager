using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.MessageDispatchers
{
    public class AzureServiceBusDispatcherService : IMessageDispatcherService
    {
        public async Task SendMessageAsync(string queueOrTopicName, string messageToSend)
        {
            ServiceBusClient client = new ServiceBusClient("Endpoint=sb://flaviotestsb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zEKMgliuem98YQYcxAHIHYTwsScRVI5mKEk957rXUCM=");

            ServiceBusSender sender = client.CreateSender(queueOrTopicName);

            ServiceBusMessage message = new ServiceBusMessage(messageToSend);

            await sender.SendMessageAsync(message);
            
            await client.DisposeAsync();
        }
    }
}
