using MySecurePasswordManager.Services;
using MySecurePasswordManager.Models;
using Xunit;
using System.IO;

namespace MySecurePasswordManager.Tests
{
    public class PasswordRepositoryTests
    {
        [Fact]
        public void AddRecord_ShouldCreateFile_AndEncryptPassword()
        {
            // Arrange
            string tempFile = Path.GetTempFileName();
            var repo = new PasswordRepository(tempFile);
            MasterPasswordService.SetMasterPassword("test123");

            // Act
            repo.AddRecord("TestTitle", "TestUser", "123456");
            var all = repo.GetAllRecords();

            // Assert
            Assert.Single(all);
            Assert.Equal("TestTitle", all[0].Title);
            Assert.Equal("TestUser", all[0].Username);
            Assert.NotEqual("123456", all[0].EncryptedPassword); // sollte verschlüsselt sein

            File.Delete(tempFile); // Aufräumen
        }
    }
}
