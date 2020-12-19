using System.Collections.Generic;
using Newtonsoft.Json;

namespace Philomena.Client.Api.Models
{
    public class ImageSearchModel
    {
        [JsonProperty("images")]
        public List<ImageModel>? Images { get; set; }

        [JsonProperty("interactions")]
        public List<InteractionModel>? Interactions { get; set; }

        [JsonProperty("total")]
        public int? Total { get; set; }
    }
}
