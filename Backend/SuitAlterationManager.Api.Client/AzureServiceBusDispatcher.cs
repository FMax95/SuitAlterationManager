using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;

namespace SuitAlterationManager.Api.Client
{
    public static class AzureServiceBusDispatcher
    {
        public static async Task SendMessageAsync(string queueOrTopicName, string messageToSend)
        {
            ServiceBusClient client = new ServiceBusClient("Endpoint=sb://flaviotestsb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=zEKMgliuem98YQYcxAHIHYTwsScRVI5mKEk957rXUCM=");

            ServiceBusSender sender = client.CreateSender(queueOrTopicName);

            ServiceBusMessage message = new ServiceBusMessage(messageToSend);

            await sender.SendMessageAsync(message);
            
            await client.DisposeAsync();
        }
    }
}
