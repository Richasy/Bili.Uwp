// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using System.Linq;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using CommunityToolkit.WinUI.Helpers;

namespace Bili.ViewModels.Desktop.Common
{
    /// <summary>
    /// 弹幕视图模型.
    /// </summary>
    public sealed partial class DanmakuModuleViewModel
    {
        private void Initialize()
        {
            IsShowDanmaku = _settingsToolkit.ReadLocalSetting(SettingNames.IsShowDanmaku, true);
            DanmakuOpacity = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuOpacity, 0.8);
            DanmakuFontSize = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuFontSize, 1.5d);
            DanmakuArea = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuArea, 1d);
            DanmakuSpeed = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuSpeed, 1d);
            DanmakuFont = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuFont, "Segoe UI");
            IsDanmakuMerge = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuMerge, false);
            IsDanmakuBold = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuBold, true);
            IsDanmakuLimit = true;
            UseCloudShieldSettings = _settingsToolkit.ReadLocalSetting(SettingNames.UseCloudShieldSettings, true);
            Location = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuLocation, Models.Enums.App.DanmakuLocation.Scroll);

            IsStandardSize = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuStandardSize, true);
            PropertyChanged += OnPropertyChanged;

            TryClear(FontCollection);
            var fontList = _fontToolkit.GetSystemFonts();
            fontList.ForEach(p => FontCollection.Add(p));

            LocationCollection.Add(Models.Enums.App.DanmakuLocation.Scroll);
            LocationCollection.Add(Models.Enums.App.DanmakuLocation.Top);
            LocationCollection.Add(Models.Enums.App.DanmakuLocation.Bottom);

            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.White), "#FFFFFF"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Red), "#FE0302"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Orange), "#FFAA02"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Khaki), "#FFD302"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Yellow), "#FFFF00"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Grass), "#A0EE00"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Green), "#00CD00"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Blue), "#019899"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Purple), "#4266BE"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.LightBlue), "#89D5FF"));

            Color = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuColor, ColorCollection.First().Value);
        }

        /// <summary>
        /// 转换为弹幕颜色.
        /// </summary>
        /// <param name="hexColor">HEX颜色.</param>
        /// <returns>颜色字符串.</returns>
        private string ToDanmakuColor(string hexColor)
        {
            var color = ColorHelper.ToColor(hexColor);
            var num = (color.R * 256 * 256) + (color.G * 256) + (color.B * 1);
            return num.ToString();
        }

        private void Seek(double seconds)
            => _currentSeconds = seconds;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsShowDanmaku):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsShowDanmaku, IsShowDanmaku);
                    break;
                case nameof(DanmakuOpacity):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuOpacity, DanmakuOpacity);
                    break;
                case nameof(DanmakuFontSize):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuFontSize, DanmakuFontSize);
                    break;
                case nameof(DanmakuArea):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuArea, DanmakuArea);
                    break;
                case nameof(DanmakuSpeed):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuSpeed, DanmakuSpeed);
                    break;
                case nameof(DanmakuFont):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuFont, DanmakuFont);
                    break;
                case nameof(IsDanmakuMerge):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsDanmakuMerge, IsDanmakuMerge);
                    break;
                case nameof(IsDanmakuBold):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsDanmakuBold, IsDanmakuBold);
                    break;
                case nameof(UseCloudShieldSettings):
                    _settingsToolkit.WriteLocalSetting(SettingNames.UseCloudShieldSettings, UseCloudShieldSettings);
                    break;
                case nameof(IsStandardSize):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsDanmakuStandardSize, IsStandardSize);
                    break;
                case nameof(Location):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuLocation, Location);
                    break;
                case nameof(Color):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuColor, Color);
                    break;
                default:
                    break;
            }
        }
    }
}
