using System.Collections;
using System.Collections.Generic;

namespace FinanceManager.Domain.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
