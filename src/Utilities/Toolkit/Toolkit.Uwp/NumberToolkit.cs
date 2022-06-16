﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.Locator.Uwp;
using Bili.Toolkit.Interfaces;

namespace Bili.Toolkit.Uwp
{
    /// <summary>
    /// 数字处理工具.
    /// </summary>
    public sealed class NumberToolkit : INumberToolkit
    {
        /// <inheritdoc/>
        public string GetCountText(double count)
        {
            if (count < 0)
            {
                return string.Empty;
            }

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
        public double GetCountNumber(string text, string removeText = "")
        {
            if (!string.IsNullOrEmpty(removeText))
            {
                text = text.Replace(removeText, string.Empty).Trim();
            }

            // 对于目前的B站来说，汉字单位只有 `万` 和 `亿` 两种.
            if (text.EndsWith("万"))
            {
                var num = Convert.ToDouble(text.Replace("万", string.Empty));
                return num * 10000;
            }
            else if (text.EndsWith("亿"))
            {
                var num = Convert.ToDouble(text.Replace("亿", string.Empty));
                return num * 100000000;
            }

            return double.TryParse(text, out var number) ? number : -1;
        }

        /// <inheritdoc/>
        public string GetDurationText(TimeSpan timeSpan)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            if (timeSpan.TotalHours > 1)
            {
                return Math.Round(timeSpan.TotalHours, 2) + " " + resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Hours);
            }
            else if (timeSpan.TotalMinutes > 1)
            {
                return Math.Round(timeSpan.TotalMinutes) + " " + resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Minutes);
            }
            else
            {
                return Math.Round(timeSpan.TotalSeconds) + " " + resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Seconds);
            }
        }

        /// <inheritdoc/>
        public string FormatDurationText(string webDurationText)
        {
            var sec = GetDurationSeconds(webDurationText);
            return GetDurationText(TimeSpan.FromSeconds(sec));
        }

        /// <inheritdoc/>
        public int GetDurationSeconds(string durationText)
        {
            var colonCount = durationText.Count(p => p == ':');
            var hourStr = string.Empty;
            if (colonCount == 1)
            {
                durationText = "00:" + durationText;
            }
            else if (colonCount == 2)
            {
                var sp = durationText.Split(':');
                durationText = string.Join(':', "00", sp[1], sp[2]);
                hourStr = sp[0];
            }

            var ts = TimeSpan.Parse(durationText);
            if (!string.IsNullOrEmpty(hourStr))
            {
                ts += TimeSpan.FromHours(Convert.ToInt32(hourStr));
            }

            return Convert.ToInt32(ts.TotalSeconds);
        }

        /// <inheritdoc/>
        public string FormatDurationText(TimeSpan ts, bool hasHours)
        {
            return hasHours
                ? $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}"
                : $"{Math.Floor(ts.TotalMinutes):00}:{ts.Seconds:00}";
        }
    }
}
