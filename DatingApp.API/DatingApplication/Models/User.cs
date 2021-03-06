using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DatingApplication.Models
{
    public class User : IdentityUser<int> // In default it uses string as Id... But we are passing int as ID;
    {
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs{ get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Like> Likers { get; set; }
        public ICollection<Like> Likees { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessageReceived { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}