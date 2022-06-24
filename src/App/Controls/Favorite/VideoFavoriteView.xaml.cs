// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;
using Bili.ViewModels.Uwp.Video;
using Splat;
using Windows.UI.Xaml;

namespace Bili.App.Controls
{
    /// <summary>
    /// 视频收藏夹视图.
    /// </summary>
    public sealed partial class VideoFavoriteView : CenterPopup
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(VideoFavoriteFolderDetailViewModel), typeof(VideoFavoriteView), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteView"/> class.
        /// </summary>
        /// <param name="vm">视图模型.</param>
        public VideoFavoriteView()
        {
            InitializeComponent();
            ViewModel = Splat.Locator.Current.GetService<VideoFavoriteFolderDetailViewModel>();
            DataContext = ViewModel;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public VideoFavoriteFolderDetailViewModel ViewModel
        {
            get { return (VideoFavoriteFolderDetailViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="folder">视频收藏夹数据模型.</param>
        public void Show(VideoFavoriteFolder folder)
        {
            ViewModel.SetFavoriteFolder(folder);
            Show();
        }
    }
}
