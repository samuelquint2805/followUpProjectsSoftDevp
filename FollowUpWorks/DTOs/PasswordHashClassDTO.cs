namespace FollowUpWorks.DTOs
{
    public class PasswordHashClassDTO
    {
        public Guid idPassword { get; set; } = Guid.NewGuid();
        
        public int Lenght { get; set; } = 0;
        public bool IncludeUppercase { get; set; } = true;
        public bool IncludeLowercase { get; set; } = true;
        public bool IncludeNumbers { get; set; } = true;
        public bool IncludeSpecialCharacters { get; set; } = true;
        public string? GeneratedPassword { get; set; } 
    }
}
