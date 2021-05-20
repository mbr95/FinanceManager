using FinanceManager.API.Formatters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FinanceManager.API.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetSerializedErrorMessages(this ModelStateDictionary dictionary)
        {
            var modelErrors = dictionary.Values.SelectMany(v => v.Errors);
            var serializedModelErrors = JsonFormatter.Serialize(modelErrors);

            return serializedModelErrors;
        }
    }
}
