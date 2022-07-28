// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Uwp.Base;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Article
{
    /// <summary>
    /// 文章收藏夹视图模型.
    /// </summary>
    public partial class ArticleFavoriteModuleViewModel : InformationFlowViewModelBase<IArticleItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleFavoriteModuleViewModel"/> class.
        /// </summary>
        public ArticleFavoriteModuleViewModel(
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _favoriteProvider = favoriteProvider;
            _resourceToolkit = resourceToolkit;
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _favoriteProvider.ResetArticleStatus();
            IsEmpty = false;
            _isEnd = false;
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestArticleFavoriteFailed)}\n{errorMsg}";

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (_isEnd)
            {
                IsEmpty = Items.Count == 0;
                return;
            }

            var data = await _favoriteProvider.GetFavortieArticleListAsync();
            foreach (var item in data.Items)
            {
                var articleVM = Locator.Current.GetService<IArticleItemViewModel>();
                articleVM.InjectData(item);
                articleVM.InjectAction(vm => RemoveItem(vm));
                Items.Add(articleVM);
            }

            IsEmpty = Items.Count == 0;
            _isEnd = Items.Count >= data.TotalCount;
        }

        private void RemoveItem(IArticleItemViewModel vm)
        {
            Items.Remove(vm);
            IsEmpty = Items.Count == 0;
        }
    }
}
