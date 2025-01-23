namespace MySecurePasswordManager.Models
{
    public class PasswordRecord
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Eindeutige ID
        public string Title { get; set; } = null!;                  // z.B. "Google"
        public string Username { get; set; } = null!;
        public string EncryptedPassword { get; set; } = null!;      // Verschlüsselte PW-Daten
    }
}
