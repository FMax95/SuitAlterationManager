using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;

namespace NotificationHub
{
    public class SendNotificationFunction
    {
        [Function("SendNotification")]
        public static async Task SendNotification(
            [ServiceBusTrigger(topicName: "AlterationFinished", subscriptionName:"emailsubscription", Connection = "ServiceBusConnectionString")] string item,        
            FunctionContext executionContext)
        {
            //var obj = JsonConvert.DeserializeObject<object>(item);
            throw new NotImplementedException();
        }
    }
}