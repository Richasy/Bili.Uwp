// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums;

namespace Bili.Toolkit.Interfaces
{
    /// <summary>
    /// application settings related toolkit.
    /// </summary>
    public interface ISettingsToolkit
    {
        /// <summary>
        /// Write local setting.
        /// </summary>
        /// <typeparam name="T">Type of written value.</typeparam>
        /// <param name="settingName">Setting name.</param>
        /// <param name="value">Setting value.</param>
        void WriteLocalSetting<T>(SettingNames settingName, T value);

        /// <summary>
        /// Read local setting.
        /// </summary>
        /// <typeparam name="T">Type of read value.</typeparam>
        /// <param name="settingName">Setting name.</param>
        /// <param name="defaultValue">Default value provided when the setting does not exist.</param>
        /// <returns>Setting value obtained.</returns>
        T ReadLocalSetting<T>(SettingNames settingName, T defaultValue);

        /// <summary>
        /// Delete local setting.
        /// </summary>
        /// <param name="settingName">Setting name.</param>
        void DeleteLocalSetting(SettingNames settingName);

        /// <summary>
        /// Whether the setting to be read has been created locally.
        /// </summary>
        /// <param name="settingName">Setting name.</param>
        /// <returns><c>true</c> means the local setting exists, <c>false</c> means it does not exist.</returns>
        bool IsSettingKeyExist(SettingNames settingName);
    }
}
