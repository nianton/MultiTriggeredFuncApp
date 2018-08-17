using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

namespace MultipleTriggeredFuncApp
{
    /// <summary>
    /// Class responsible to process a single message for triggered dequeue function. 
    /// All the logic is to kept outside of the T4 template file for ease of development.
    /// </summary>
    public class DequeueMessageProcessor
    {
        private readonly BrokeredMessage _message;
        private readonly IAsyncCollector<MessageTrackingInfo> _cosmosCollector;
        private readonly TraceWriter _log;
        private readonly string _functionName;

        public DequeueMessageProcessor(BrokeredMessage message, IAsyncCollector<MessageTrackingInfo> cosmosCollector, TraceWriter log, string functionNameTriggered)
        {
            _message = message;
            _cosmosCollector = cosmosCollector;
            _log = log;
            _functionName = functionNameTriggered;
        }

        public async Task ProcessAsync()
        {
            _log.Info($"C# ServiceBus queue trigger function named: {_functionName}, processed message: {_message.GetBody<string>()}");

            var trackingInfo = new MessageTrackingInfo()
            {
                Id = _message.MessageId,
                Payload = _message.GetBody<string>()
            };

            await _cosmosCollector.AddAsync(trackingInfo);
        }
    }
}
