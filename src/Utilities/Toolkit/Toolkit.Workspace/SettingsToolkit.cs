// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Windows.Storage;

namespace Bili.Toolkit.Workspace
{
    /// <summary>
    /// Settings toolkit.
    /// </summary>
    public class SettingsToolkit : ISettingsToolkit
    {
        /// <inheritdoc/>
        public T ReadLocalSetting<T>(SettingNames settingName, T defaultValue)
        {
            var settingContainer = ApplicationData.Current.LocalSettings;

            if (IsSettingKeyExist(settingName))
            {
                if (defaultValue is Enum)
                {
                    var tempValue = settingContainer.Values[settingName.ToString()].ToString();
                    Enum.TryParse(typeof(T), tempValue, out var result);
                    return (T)result;
                }
                else
                {
                    return (T)settingContainer.Values[settingName.ToString()];
                }
            }
            else
            {
                WriteLocalSetting(settingName, defaultValue);
                return defaultValue;
            }
        }

        /// <inheritdoc/>
        public void WriteLocalSetting<T>(SettingNames settingName, T value)
        {
            var settingContainer = ApplicationData.Current.LocalSettings;

            if (value is Enum)
            {
                settingContainer.Values[settingName.ToString()] = value.ToString();
            }
            else
            {
                settingContainer.Values[settingName.ToString()] = value;
            }
        }

        /// <inheritdoc/>
        public void DeleteLocalSetting(SettingNames settingName)
        {
            var settingContainer = ApplicationData.Current.LocalSettings;

            if (IsSettingKeyExist(settingName))
            {
                settingContainer.Values.Remove(settingName.ToString());
            }
        }

        /// <inheritdoc/>
        public bool IsSettingKeyExist(SettingNames settingName)
            => ApplicationData.Current.LocalSettings.Values.ContainsKey(settingName.ToString());
    }
}
