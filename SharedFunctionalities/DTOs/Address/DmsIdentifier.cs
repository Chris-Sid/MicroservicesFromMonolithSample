using Newtonsoft.Json;

namespace SharedFunctionalities.DTOs.Address
{
    public partial class DmsIdentifier
    {
        /// <summary>
        /// Internal Id (the DMS ID)
        /// </summary>
        [JsonProperty("internalId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string InternalId { get; set; }

        /// <summary>
        /// External Id (the DSW/NMT ID)
        /// </summary>
        [Newtonsoft.Json.JsonProperty("externalId", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ExternalId { get; set; }

    }
}
