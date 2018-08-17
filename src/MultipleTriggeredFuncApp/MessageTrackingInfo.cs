using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleTriggeredFuncApp
{
    /// <summary>
    /// Simple class to act as the CosmosDB document.
    /// </summary>
    public class MessageTrackingInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }
}
