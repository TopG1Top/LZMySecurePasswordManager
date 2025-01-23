using System.Text.Json;
using MySecurePasswordManager.Models;

namespace MySecurePasswordManager.Services
{
    public class PasswordRepository
    {
        private readonly string _filePath;
        private readonly EncryptionService _encryption;

        public PasswordRepository(string filePath)
        {
            _filePath = filePath;
            _encryption = new EncryptionService();
        }

        private List<PasswordRecord> LoadRecords()
        {
            if (!File.Exists(_filePath))
                return new List<PasswordRecord>();

            var json = File.ReadAllText(_filePath);
            var records = JsonSerializer.Deserialize<List<PasswordRecord>>(json);
            return records ?? new List<PasswordRecord>();
        }

        private void SaveRecords(List<PasswordRecord> records)
        {
            var json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void AddRecord(string title, string username, string plainPassword)
        {
            var masterKey = MasterPasswordService.GetMasterPassword();
            if (string.IsNullOrEmpty(masterKey))
                throw new InvalidOperationException("Master password not set.");

            var records = LoadRecords();
            var encrypted = _encryption.Encrypt(plainPassword, masterKey);

            var newRecord = new PasswordRecord
            {
                Title = title,
                Username = username,
                EncryptedPassword = encrypted
            };
            records.Add(newRecord);
            SaveRecords(records);
        }

        public List<PasswordRecord> GetAllRecords()
        {
            return LoadRecords();
        }

        public PasswordRecord? GetRecordById(string id)
        {
            var records = LoadRecords();
            return records.FirstOrDefault(r => r.Id == id);
        }

        public void RemoveRecord(string id)
        {
            var records = LoadRecords();
            var record = records.FirstOrDefault(r => r.Id == id);
            if (record != null)
            {
                records.Remove(record);
                SaveRecords(records);
            }
        }

        public string? DecryptPassword(string id)
        {
            var masterKey = MasterPasswordService.GetMasterPassword();
            if (string.IsNullOrEmpty(masterKey))
                throw new InvalidOperationException("Master password not set.");

            var record = GetRecordById(id);
            if (record == null) return null;

            return new EncryptionService().Decrypt(record.EncryptedPassword, masterKey);
        }
    }
}
