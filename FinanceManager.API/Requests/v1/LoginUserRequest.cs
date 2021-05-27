using System.ComponentModel.DataAnnotations;

namespace FinanceManager.API.Requests.v1
{
    public class LoginUserRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
