using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MojangSharpCore.Responses
{
    public class HasJoinResponse : Response
    {
        internal HasJoinResponse(Response response) : base(response)
        {
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        public class Property
        {
            /// <summary>
            /// Property name
            /// </summary>
            [JsonProperty("name")]
            public string Name { get; internal set; }

            /// <summary>
            /// Property value
            /// </summary>
            [JsonProperty("value")]
            public string Value { get; internal set; }
        }
    }
}
