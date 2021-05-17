using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.API.Options
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifetime { get; set; }
    }
}
