// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Enums;

namespace Bili.ViewModels.Interfaces.Search
{
    /// <summary>
    /// 搜索模块条目视图模型的接口定义.
    /// </summary>
    public interface ISearchModuleItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 类型.
        /// </summary>
        SearchModuleType Type { get; }

        /// <summary>
        /// 标题.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 是否可用.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// 设置数据.
        /// </summary>
        /// <param name="type">搜索模块类型.</param>
        /// <param name="title">标题栏.</param>
        /// <param name="isEnabled">是否启用.</param>
        void SetData(SearchModuleType type, string title, bool isEnabled = true);
    }
}
