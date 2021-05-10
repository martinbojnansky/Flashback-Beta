using Windows.Storage;

namespace Helpers.Storage
{
    public static class RoamingSettingsHelper
    {
        private static ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
        private static StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue(string key, object value)
        {
            if (roamingSettings.Values.ContainsKey(key))
            {
                roamingSettings.Values[key] = value;
            }
            else
            {
                roamingSettings.Values.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(string key)
        {
            if (roamingSettings.Values.ContainsKey(key))
            {
                return roamingSettings.Values[key];
            }
            else
            {
                return null;
            }
        }
    }
}
