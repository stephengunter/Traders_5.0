using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Views
{
    public abstract class BaseAuthorityRequest
    {
        [Required(ErrorMessage = "必須填寫密碼")]
        public string Password { get; set; }
    }
}
