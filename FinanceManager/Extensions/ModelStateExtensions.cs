using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.Extensions
{
    public static class ModelStateExtensions
    {
        public static IEnumerable<ModelError> GetErrorMessages(this ModelStateDictionary dictionary)
        {
            return dictionary.Values.SelectMany(v => v.Errors);
        }
    }
}
