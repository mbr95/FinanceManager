using System.ComponentModel.DataAnnotations;

namespace FinanceManager.API.Requests.v1
{
    public class RegisterUserRequest
    {
        [Required]
        [MinLength(5)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
