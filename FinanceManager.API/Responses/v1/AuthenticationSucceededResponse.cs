using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManager.API.Responses.v1
{
    public class AuthenticationSucceededResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
