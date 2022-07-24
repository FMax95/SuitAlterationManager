using System.Threading.Tasks;

namespace SuitAlterationManager.Infrastructure.MessageDispatchers
{
    public interface IMessageDispatcherService
    {
        Task SendMessageAsync(string queueOrTopicName, string messageToSend);
    }
}
