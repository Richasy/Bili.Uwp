// Copyright (c) GodLeaveMe. All rights reserved.

using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 横幅视图模型.
    /// </summary>
    public partial class BannerViewModel
    {
        /// <summary>
        /// 导航地址.
        /// </summary>
        [Reactive]
        public string Uri { get; set; }

        /// <summary>
        /// 封面地址.
        /// </summary>
        [Reactive]
        public string Cover { get; set; }

        /// <summary>
        /// 描述文本.
        /// </summary>
        [Reactive]
        public string Description { get; set; }

        /// <summary>
        /// 悬浮提示是否可用.
        /// </summary>
        [Reactive]
        public bool IsTooltipEnabled { get; set; }

        /// <summary>
        /// 源对象.
        /// </summary>
        public object Source { get; private set; }

        /// <summary>
        /// 最小高度.
        /// </summary>
        [Reactive]
        public double MinHeight { get; set; }
    }
}
