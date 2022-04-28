// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bilibili.App.Dynamic.V2;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 文章动态.
    /// </summary>
    public sealed partial class DynamicArticleItem : UserControl
    {
        /// <summary>
        /// <see cref="Data"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(MdlDynArticle), typeof(DynamicArticleItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicArticleItem"/> class.
        /// </summary>
        public DynamicArticleItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 数据.
        /// </summary>
        public MdlDynArticle Data
        {
            get { return (MdlDynArticle)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is MdlDynArticle data)
            {
                var instance = d as DynamicArticleItem;
                instance.TitleBlock.Text = data.Title;
                instance.DescriptionBlock.Visibility = !string.IsNullOrEmpty(data.Desc) ? Visibility.Visible : Visibility.Collapsed;
                instance.DescriptionBlock.Text = data.Desc;

                instance.CoverImage.Visibility = data.Covers.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                if (data.Covers.Count > 0)
                {
                    instance.CoverImage.ImageUrl = data.Covers.First();
                }

                instance.AdditionalBlock.Text = data.Label;
            }
        }
    }
}
