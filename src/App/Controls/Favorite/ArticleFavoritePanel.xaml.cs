// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Splat;

namespace Bili.App.Controls.Favorite
{
    /// <summary>
    /// 文章收藏夹.
    /// </summary>
    public sealed partial class ArticleFavoritePanel : ArticleFavoritePanelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleFavoritePanel"/> class.
        /// </summary>
        public ArticleFavoritePanel()
        {
            InitializeComponent();
            var vm = Splat.Locator.Current.GetService<ArticleFavoriteModuleViewModel>();
            ViewModel = vm;
            DataContext = vm;
        }

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public AppViewModel CoreViewModel { get; } = Splat.Locator.Current.GetService<AppViewModel>();
    }

    /// <summary>
    /// <see cref="ArticleFavoritePanel"/> 的基类.
    /// </summary>
    public class ArticleFavoritePanelBase : ReactiveUserControl<ArticleFavoriteModuleViewModel>
    {
    }
}
