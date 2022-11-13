// Copyright (c) Richasy. All rights reserved.
using Bili.Toolkit.Interfaces;

namespace Bili.Toolkit.Desktop
{
    /// <summary>
    /// 文本工具.
    /// </summary>
    public sealed class TextToolkit : ITextToolkit
    {
        private readonly ISettingsToolkit _settingsToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextToolkit"/> class.
        /// </summary>
        public TextToolkit(ISettingsToolkit settingsToolkit)
            => _settingsToolkit = settingsToolkit;

        /// <inheritdoc/>
        public string ConvertToTraditionalChineseIfNeeded(string text)
        {
            var lan = _settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.LastAppLanguage, "zh-Hans-CN");
            var needConvert = _settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.IsFullTraditionalChinese, false);

            if (!lan.Contains("zh-hant", System.StringComparison.OrdinalIgnoreCase)
                || !needConvert
                || string.IsNullOrEmpty(text))
            {
                return text;
            }

            var type = 0;
            if (lan.Contains("hk", System.StringComparison.OrdinalIgnoreCase))
            {
                // 香港地区.
                type = 1;
            }
            else if (lan.Contains("tw", System.StringComparison.OrdinalIgnoreCase))
            {
                // 台湾地区.
                type = 2;
            }

            return ToolGood.Words.WordsHelper.ToTraditionalChinese(text, type);
        }
    }
}
