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
            try
            {
                var localeList = new List<string>
                {
                    "zh-cn",
                };
                return CanvasTextFormat.GetSystemFontFamilies(localeList).OrderBy(x => x).ToList();
            }
            catch (System.Exception)
            {
                return new List<string>
                {
                    "Segoe UI",
                };
            }
        }
    }
}
