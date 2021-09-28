// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频播放器.
    /// </summary>
    public partial class BiliPlayer
    {
        /// <summary>
        /// <see cref="CoverUrl"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty CoverUrlProperty =
            DependencyProperty.Register(nameof(CoverUrl), typeof(string), typeof(BiliPlayer), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="ViewModel"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PlayerViewModel), typeof(BiliPlayer), new PropertyMetadata(PlayerViewModel.Instance));

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PlayerViewModel ViewModel
        {
            get { return (PlayerViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 封面地址.
        /// </summary>
        public string CoverUrl
        {
            get { return (string)GetValue(CoverUrlProperty); }
            set { SetValue(CoverUrlProperty, value); }
        }
    }
}
