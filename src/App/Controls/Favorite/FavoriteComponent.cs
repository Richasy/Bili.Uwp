// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 收藏夹组件.
    /// </summary>
    public class FavoriteComponent : UserControl
    {
        /// <summary>
        /// 视图模型.
        /// </summary>
        public FavoriteViewModel ViewModel { get; } = FavoriteViewModel.Instance;
    }
}
