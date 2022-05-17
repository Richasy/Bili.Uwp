// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums;
using Bili.Models.Enums.App;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// 应用导航返回事件参数.
    /// </summary>
    public sealed class AppBackEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBackEventArgs"/> class.
        /// </summary>
        /// <param name="type">在哪一个导航层级中返回.</param>
        /// <param name="pageId">返回前加载的页面 Id.</param>
        /// <param name="parameter">回退附带的导航参数.</param>
        public AppBackEventArgs(
            NavigationType type,
            PageIds pageId,
            object parameter)
        {
            FromType = type;
            FromPageId = pageId;
            BackParameter = parameter;
        }

        /// <summary>
        /// 在哪一个导航层级中返回.
        /// </summary>
        public NavigationType FromType { get; }

        /// <summary>
        /// 返回前加载的页面 Id.
        /// </summary>
        public PageIds FromPageId { get; }

        /// <summary>
        /// 回退附带的导航参数.
        /// </summary>
        public object BackParameter { get; }
    }
}
