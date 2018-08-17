using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MultipleTriggeredFuncApp
{
    public static class DequeueGeneratedFunction
    {
        private const string QueueName = "myqueue";
        private const string CosmosConnectionString = "<COSMOSDB_CONNECTION>";
        private const string CosmosDbName = "mainDocuments";
        private const string CosmosCollectionName = "messageCollection";

        [FunctionName(nameof(DequeueFromWESTEUROPE))]
        public static async Task DequeueFromWESTEUROPE(
            [ServiceBusTrigger(QueueName, AccessRights.Manage, Connection = "SBCONNECTION1")]
            BrokeredMessage message,
            [DocumentDB(CosmosDbName, CosmosCollectionName, ConnectionStringSetting = CosmosConnectionString)]
            IAsyncCollector<MessageTrackingInfo> cosmosCollector,
            TraceWriter log,
			[CallerMemberName]
			string functionName = null)
        {
			var processor = new DequeueMessageProcessor(message, cosmosCollector, log, functionName);
            await processor.ProcessAsync();
        }

        [FunctionName(nameof(DequeueFromNORTHEUROPE))]
        public static async Task DequeueFromNORTHEUROPE(
            [ServiceBusTrigger(QueueName, AccessRights.Manage, Connection = "SBCONNECTION2")]
            BrokeredMessage message,
            [DocumentDB(CosmosDbName, CosmosCollectionName, ConnectionStringSetting = CosmosConnectionString)]
            IAsyncCollector<MessageTrackingInfo> cosmosCollector,
            TraceWriter log,
			[CallerMemberName]
			string functionName = null)
        {
			var processor = new DequeueMessageProcessor(message, cosmosCollector, log, functionName);
            await processor.ProcessAsync();
        }

        [FunctionName(nameof(DequeueFromSOUTHUK))]
        public static async Task DequeueFromSOUTHUK(
            [ServiceBusTrigger(QueueName, AccessRights.Manage, Connection = "SBCONNECTION3")]
            BrokeredMessage message,
            [DocumentDB(CosmosDbName, CosmosCollectionName, ConnectionStringSetting = CosmosConnectionString)]
            IAsyncCollector<MessageTrackingInfo> cosmosCollector,
            TraceWriter log,
			[CallerMemberName]
			string functionName = null)
        {
			var processor = new DequeueMessageProcessor(message, cosmosCollector, log, functionName);
            await processor.ProcessAsync();
        }

    }
}
