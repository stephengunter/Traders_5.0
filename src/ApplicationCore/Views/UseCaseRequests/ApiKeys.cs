using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Views
{
    public class ApiKeyRequest : BaseAuthorityRequest
    {
        public bool UserHasPassword { get; set; }
    }
}
