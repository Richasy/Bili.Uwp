// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.Toolkit.Uwp
{
    /// <summary>
    /// 数字处理工具.
    /// </summary>
    public class NumberToolkit : INumberToolkit
    {
        /// <inheritdoc/>
        public string GetCountText(double count)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            if (count >= 100000000)
            {
                var unit = resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Billion);
                return Math.Round(count / 100000000, 2) + unit;
            }
            else if (count >= 10000)
            {
                var unit = resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.TenThousands);
                return Math.Round(count / 10000, 2) + unit;
            }

            return count.ToString();
        }

        /// <inheritdoc/>
        public string GetDurationText(TimeSpan timeSpan)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            if (timeSpan.TotalHours > 1)
            {
                return Math.Round(timeSpan.TotalHours, 2) + resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Hours);
            }
            else if (timeSpan.TotalMinutes > 1)
            {
                return Math.Round(timeSpan.TotalMinutes) + resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Minutes);
            }
            else
            {
                return Math.Round(timeSpan.TotalSeconds) + resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Seconds);
            }
        }
    }
}
