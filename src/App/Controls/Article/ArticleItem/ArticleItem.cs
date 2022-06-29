// Copyright (c) Richasy. All rights reserved.

using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Article;
using Splat;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Article
{
    /// <summary>
    /// 文章条目.
    /// </summary>
    public sealed class ArticleItem : ReactiveControl<ArticleItemViewModel>, IRepeaterItem, IOrientationControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleItem"/> class.
        /// </summary>
        public ArticleItem() => DefaultStyleKey = typeof(ArticleItem);

        /// <inheritdoc/>
        public Size GetHolderSize() => new(210, 248);

        /// <inheritdoc/>
        public void ChangeLayout(Orientation orientation)
        {
            var resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
            Style = orientation == Orientation.Horizontal
                ? resourceToolkit.GetResource<Style>("HorizontalArticleItemStyle")
                : resourceToolkit.GetResource<Style>("VerticalArticleItemStyle");
        }
    }
}
