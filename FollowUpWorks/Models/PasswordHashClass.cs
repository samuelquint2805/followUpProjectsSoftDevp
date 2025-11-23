using FollowUpWorks.services.Abstractions;

namespace FollowUpWorks.Models
{
    public class PasswordHashClass : iID
    {
       
        public Guid idPassword { get; set; }
        public Guid Id { get; set; }
        public int Lenght { get; set; } = 0;
        public bool IncludeUppercase { get; set; } = true;
        public bool IncludeLowercase { get; set; } = true;
        public bool IncludeNumbers { get; set; } = true;
        public bool IncludeSpecialCharacters { get; set; } = true;

        public string GeneratedPassword { get; set; } = string.Empty;
    }
}
