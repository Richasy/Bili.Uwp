// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 弹幕视图模型.
    /// </summary>
    public sealed partial class DanmakuModuleViewModel
    {
        /// <summary>
        /// 转换为弹幕颜色.
        /// </summary>
        /// <param name="hexColor">HEX颜色.</param>
        /// <returns>颜色字符串.</returns>
        private string ToDanmakuColor(string hexColor)
        {
            var color = Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(hexColor);
            var num = (color.R * 256 * 256) + (color.G * 256) + (color.B * 1);
            return num.ToString();
        }
    }
}
