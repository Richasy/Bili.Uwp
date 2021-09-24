// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
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
