// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Data.Community;

namespace Bili.ViewModels.Interfaces.Common
{
    /// <summary>
    /// 横幅视图模型的接口定义.
    /// </summary>
    public interface IBannerViewModel : INotifyPropertyChanged, IInjectDataViewModel<BannerIdentifier>
    {
        /// <summary>
        /// 导航地址.
        /// </summary>
        string Uri { get; }

        /// <summary>
        /// 封面地址.
        /// </summary>
        string Cover { get; }

        /// <summary>
        /// 描述文本.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 悬浮提示是否可用.
        /// </summary>
        bool IsTooltipEnabled { get; }

        /// <summary>
        /// 最小高度.
        /// </summary>
        double MinHeight { get; }
    }
}
