// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bili.Toolkit.Interfaces;
using Microsoft.Graphics.Canvas.Text;

namespace Bili.Toolkit.Uwp
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
