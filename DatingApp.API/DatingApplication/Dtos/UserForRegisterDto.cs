using System.ComponentModel.DataAnnotations;

namespace DatingApplication.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify password 4 and 8 charecter")]
        public string Password { get; set; }
    }
}