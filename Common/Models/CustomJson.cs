using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common.Models
{
    public class CustomJson : JsonResult
    {
        public object Data { get; set; }

        public Formatting Formatting { get; set; }

        public CustomJson(object data, Formatting formatting)
            : this(data)
        {
            Formatting = formatting;
        }

        public CustomJson(object data) : base(data)
        {
            Formatting = Formatting.None;
            SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
            };
        }
    }
}
