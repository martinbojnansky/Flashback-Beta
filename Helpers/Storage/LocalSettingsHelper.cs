using System;
using System.Collections.Generic;
using Windows.Storage;

namespace Helpers.Storage
{
    public static class LocalSettingsHelper
    {
        private static ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="container"></param>
        public static void SetValue(string key, object value, string container = null)
        {
            ApplicationDataContainer applicationDataContainer;

            if (container != null)
            {
                if (localSettings.Containers.ContainsKey(container))
                {
                    applicationDataContainer = localSettings.Containers[container];
                }
                else
                {
                    applicationDataContainer = localSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always);
                }
            }
            else
            {
                applicationDataContainer = localSettings;
            }


            if (applicationDataContainer.Values.ContainsKey(key))
            {
                applicationDataContainer.Values[key] = value;
            }
            else
            {
                applicationDataContainer.Values.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static object GetValue(string key, string container = null)
        {
            ApplicationDataContainer applicationDataContainer;

            if (container != null)
            {
                if (localSettings.Containers.ContainsKey(container))
                {
                    applicationDataContainer = localSettings.Containers[container];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            else
            {
                applicationDataContainer = localSettings;
            }

            if (applicationDataContainer.Values.ContainsKey(key))
            {
                return applicationDataContainer.Values[key];
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public static object GetContainerValues(string container)
        {
            if (localSettings.Containers.ContainsKey(container))
            {
                return localSettings.Containers[container].Values;
            }
            else
            {
                return localSettings.CreateContainer(container, ApplicationDataCreateDisposition.Always).Values;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="container"></param>
        public static void RemoveElement(string key, string container = null)
        {
            ApplicationDataContainer applicationDataContainer;

            if (container != null)
            {
                if (localSettings.Containers.ContainsKey(container))
                {
                    applicationDataContainer = localSettings.Containers[container];
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            else
            {
                applicationDataContainer = localSettings;
            }

            if (applicationDataContainer.Values.ContainsKey(key))
            {
                applicationDataContainer.Values.Remove(key);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public static void RemoveContainer(string container)
        {
            if (localSettings.Containers.ContainsKey(container))
            {
                localSettings.DeleteContainer(container);
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
