using System.Security.Cryptography;
using System.Text;

namespace MySecurePasswordManager.Services
{
    public class EncryptionService
    {
        // Verschlüsselt einen Klartext mit AES (Symmetrisch)
        public string Encrypt(string plainText, string key)
        {
            using var aes = Aes.Create();
            // Aus dem key z.B. über SHA256 einen Byte-Array machen:
            aes.Key = GetKeyBytes(key);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // IV + Ciphertext zusammenfassen -> Base64
            var result = Convert.ToBase64String(aes.IV.Concat(cipherBytes).ToArray());
            return result;
        }

        // Entschlüsselt den Base64-codierten Ciphertext
        public string Decrypt(string cipherText, string key)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = GetKeyBytes(key);

            // Ersten 16 Bytes (Standard-Blocksize) sind die IV
            byte[] iv = fullCipher.Take(aes.BlockSize / 8).ToArray();
            byte[] actualCipher = fullCipher.Skip(aes.BlockSize / 8).ToArray();

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var decryptedBytes = decryptor.TransformFinalBlock(actualCipher, 0, actualCipher.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        private byte[] GetKeyBytes(string key)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(key));
        }
    }
}
