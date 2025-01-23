using MySecurePasswordManager.Services;
using Xunit;

namespace MySecurePasswordManager.Tests
{
    public class EncryptionServiceTests
    {
        [Fact]
        public void Encrypt_And_Decrypt_ShouldReturnOriginalText()
        {
            // Arrange
            var service = new EncryptionService();
            string key = "MasterPassword123!";
            string original = "GeheimesPasswort";

            // Act
            var encrypted = service.Encrypt(original, key);
            var decrypted = service.Decrypt(encrypted, key);

            // Assert
            Assert.Equal(original, decrypted);
        }
    }
}

