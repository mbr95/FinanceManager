using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.API.Formatters
{
    public static class JsonFormatter
    {        
        public static string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data, CreateJsonSerializerSettings());           
        }

        private static JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonSerializerSettings.Formatting = Formatting.None;

            return jsonSerializerSettings;
        }
    }
}
