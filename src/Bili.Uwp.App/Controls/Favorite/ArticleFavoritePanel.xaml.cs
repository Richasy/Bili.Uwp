// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Core;

namespace Bili.Uwp.App.Controls.Favorite
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
            var vm = Locator.Instance.GetService<IArticleFavoriteModuleViewModel>();
            ViewModel = vm;
            DataContext = vm;
        }

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public IAppViewModel CoreViewModel { get; } = Locator.Instance.GetService<IAppViewModel>();
    }

    /// <summary>
    /// <see cref="ArticleFavoritePanel"/> 的基类.
    /// </summary>
    public class ArticleFavoritePanelBase : ReactiveUserControl<IArticleFavoriteModuleViewModel>
    {
    }
}
