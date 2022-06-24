// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
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
        /// <param name="behavior">后退的行为标识.</param>
        /// <param name="backAction">后退的具体执行.</param>
        /// <param name="parameter">回退附带的导航参数.</param>
        public AppBackEventArgs(
            BackBehavior behavior,
            Action<object> backAction,
            object parameter)
        {
            Id = behavior;
            Action = backAction;
            Parameter = parameter;
        }

        /// <summary>
        /// 在哪一个导航层级中返回.
        /// </summary>
        public BackBehavior Id { get; }

        /// <summary>
        /// 返回前加载的页面 Id.
        /// </summary>
        public Action<object> Action { get; }

        /// <summary>
        /// 回退附带的导航参数.
        /// </summary>
        public object Parameter { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is AppBackEventArgs args && Id == args.Id && EqualityComparer<object>.Default.Equals(Parameter, args.Parameter);

        /// <inheritdoc/>
        public override int GetHashCode()
            => Id.GetHashCode() + Parameter?.GetHashCode() ?? 0;
    }
}
