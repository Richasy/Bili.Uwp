// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.Generic;

namespace Richasy.Bili.Toolkit.Interfaces
{
    /// <summary>
    /// 字体处理工具.
    /// </summary>
    public interface IFontToolkit
    {
        /// <summary>
        /// 获取当前系统字体列表.
        /// </summary>
        /// <returns>字体名列表.</returns>
        List<string> GetSystemFontList();
    }
}
