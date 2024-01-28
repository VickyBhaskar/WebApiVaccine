using System;
using System.Collections.Generic;

namespace VaccineApi.Models
{
    public partial class UserDetail
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}
