// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 播放器组件基类.
    /// </summary>
    public class PlayerComponent : UserControl
    {
        /// <summary>
        /// 播放器视图模型.
        /// </summary>
        public PlayerViewModel ViewModel { get; } = PlayerViewModel.Instance;
    }
}
