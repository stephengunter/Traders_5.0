using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Views
{
   
    public class SetPasswordRequest
    {
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "必須填寫密碼")]
        [StringLength(12, ErrorMessage = "密碼長度超出限制")]
        public string Password { get; set; }

        public string Rule { get; set; }

        public bool UserHasPassword { get; set; }
    }

}
