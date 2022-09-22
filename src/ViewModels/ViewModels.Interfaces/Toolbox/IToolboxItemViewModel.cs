// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Enums;

namespace Bili.ViewModels.Interfaces.Toolbox
{
    /// <summary>
    /// 工具箱条目视图模型的接口定义.
    /// </summary>
    public interface IToolboxItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 类型.
        /// </summary>
        ToolboxItemType Type { get; }

        /// <summary>
        /// 标题.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 描述.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 设置工具类型.
        /// </summary>
        /// <param name="type">工具类型.</param>
        void SetType(ToolboxItemType type);
    }
}
