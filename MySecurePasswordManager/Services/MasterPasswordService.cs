namespace MySecurePasswordManager.Services
{
    public static class MasterPasswordService
    {
        private static string? _masterPassword;

        public static void SetMasterPassword(string password)
        {
            // Du könntest hier noch Validierungen etc. machen
            _masterPassword = password;
        }

        public static string? GetMasterPassword()
        {
            return _masterPassword;
        }
    }
}
