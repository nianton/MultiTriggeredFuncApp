using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MultipleTriggeredFuncApp
{
    public static class DequeueFunction
    {
        private const string QueueName = "myqueue";
        private const string PrimaryConnectionStringAppSettingName = "PRIMARYSB_CONNECTION";
        private const string SecondaryConnectionStringAppSettingName = "SECONDARYSB_CONNECTION";
        private const string CosmosConnectionString = "<COSMOSDB_CONNECTION>";
        private const string CosmosDbName = "mainDocuments";
        private const string CosmosCollectionName = "messageCollection";
        
        /// <summary>
        /// Azure Function for the primary ServiceBus instances.
        /// </summary>
        [FunctionName(nameof(Dequeue))]
        public static async Task Dequeue(
            [ServiceBusTrigger(QueueName, AccessRights.Manage, Connection = PrimaryConnectionStringAppSettingName)]
            BrokeredMessage message,
            [DocumentDB(CosmosDbName, CosmosCollectionName, ConnectionStringSetting = CosmosConnectionString)]
            IAsyncCollector<MessageTrackingInfo> cosmosCollector,
            TraceWriter log)
        {
            await DoDequeue(message, cosmosCollector, log);
        }

        /// <summary>
        /// Azure Function for the secondary ServiceBus instances.
        /// </summary>
        [FunctionName(nameof(DequeueSecondary))]
        public static async Task DequeueSecondary(
            [ServiceBusTrigger(QueueName, AccessRights.Manage, Connection = SecondaryConnectionStringAppSettingName)]
            BrokeredMessage message,
            [DocumentDB(CosmosDbName, CosmosCollectionName, ConnectionStringSetting = CosmosConnectionString)]
            IAsyncCollector<MessageTrackingInfo> cosmosCollector,
            TraceWriter log)
        {
            await DoDequeue(message, cosmosCollector, log);
        }

        /// <summary>
        /// Method (or it could be part of an external library/project) that contains the logic of the dequeue function.
        /// </summary>
        private static async Task DoDequeue(BrokeredMessage message, IAsyncCollector<MessageTrackingInfo> cosmosCollector, TraceWriter log, [CallerMemberName]string functionName = null)
        {
            log.Info($"C# ServiceBus queue trigger function named: {functionName}, processed message: {message.GetBody<string>()}");

            var trackingInfo = new MessageTrackingInfo()
            {
                Id  = message.MessageId,
                Payload = message.GetBody<string>()
            };

            await cosmosCollector.AddAsync(trackingInfo);
        }
    }
}
