using Infrastructure.Entities;
using ApplicationCore.Helpers;
using ApplicationCore.Auth.ApiKey;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class ApiKey : BaseRecord
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Key { get; set; }

        public User User { get; set; }
        public ICollection<string> Roles => Role.SplitToList();
    }

}
