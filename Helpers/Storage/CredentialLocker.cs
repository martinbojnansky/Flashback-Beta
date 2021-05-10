using Windows.Security.Credentials;

namespace Helpers.Storage
{
    public static class CredentialLocker
    {
        private static PasswordVault vault = new PasswordVault();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static void AddCredential(string resource, string userName, string password)
        {
            vault.Add(new PasswordCredential(resource, userName, password));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static PasswordCredential GetCredential(string resource, string userName)
        {
            return vault.Retrieve(resource, userName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="credential"></param>
        public static void RemoveCredential(PasswordCredential credential)
        {
            vault.Remove(credential);
        }
    }
}
