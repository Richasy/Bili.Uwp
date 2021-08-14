// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Graphics.Canvas.Text;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.Toolkit.Uwp
{
    /// <summary>
    /// 字体处理工具.
    /// </summary>
    public class FontToolkit : IFontToolkit
    {
        /// <inheritdoc/>
        public List<string> GetSystemFontList()
        {
            var defaultLan = "en-us";
            var fonts = CanvasFontSet.GetSystemFontSet();
            var result = new List<string>();
            foreach (var font in fonts.Fonts)
            {
                font.FamilyNames.TryGetValue(defaultLan, out var fontName);
                if (string.IsNullOrEmpty(fontName) && font.FamilyNames.Count > 0)
                {
                    fontName = font.FamilyNames.First().Value;
                }

                if (!string.IsNullOrEmpty(fontName) && !result.Contains(fontName))
                {
                    result.Add(fontName);
                }
            }

            return result;
        }
    }
}
