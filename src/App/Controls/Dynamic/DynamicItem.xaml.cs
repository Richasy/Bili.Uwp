// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 动态条目.
    /// </summary>
    public sealed partial class DynamicItem : UserControl, IDynamicLayoutItem, IRepeaterItem
    {
        /// <summary>
        /// <see cref="Data"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(Bilibili.App.Dynamic.V2.DynamicItem), typeof(DynamicItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// <see cref="Orientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(DynamicItem), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItem"/> class.
        /// </summary>
        public DynamicItem()
        {
            this.InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 数据.
        /// </summary>
        public Bilibili.App.Dynamic.V2.DynamicItem Data
        {
            get { return (Bilibili.App.Dynamic.V2.DynamicItem)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// 布局方式.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <inheritdoc/>
        public Size GetHolderSize()
        {
            if (Orientation == Orientation.Horizontal)
            {
                return new Size(double.PositiveInfinity, 180);
            }
            else
            {
                return new Size(320, 248);
            }
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DynamicItem;
            instance.CheckOrientation();
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Bilibili.App.Dynamic.V2.DynamicItem data)
            {
                var instance = d as DynamicItem;
                var modules = data.Modules.ToList();
                var userModule = modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleAuthor).FirstOrDefault()?.ModuleAuthor;
                var descModule = modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleDesc).FirstOrDefault()?.ModuleDesc;
                var mainModule = modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleDynamic).FirstOrDefault()?.ModuleDynamic;
                var dataModule = modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleStat).FirstOrDefault()?.ModuleStat;

                if (userModule != null)
                {
                    instance.UserAvatar.Avatar = userModule.Author.Face;
                    instance.UserAvatar.UserName = userModule.Author.Name;
                    instance.UserNameBlock.Text = userModule.Author.Name;
                    instance.DateBlock.Text = userModule.PtimeLabelText;
                }

                if (descModule != null)
                {
                    if (string.IsNullOrEmpty(descModule.Text.Trim()))
                    {
                        instance.MainContainer.Margin = new Thickness(0, -36, 0, 0);
                    }
                    else
                    {
                        instance.MainContainer.Margin = new Thickness(0);
                    }

                    instance.DescriptionBlock.Text = descModule.Text.Trim();
                }
                else
                {
                    instance.MainContainer.Margin = new Thickness(0, -36, 0, 0);
                    instance.DescriptionBlock.Text = string.Empty;
                }

                if (mainModule != null)
                {
                    if (mainModule.Type == Bilibili.App.Dynamic.V2.ModuleDynamicType.MdlDynPgc)
                    {
                        instance.AddViewLaterButton.IsEnabled = false;
                        instance.MainContentPresenter.Content = new SeasonViewModel(mainModule.DynPgc);
                        instance.MainContentPresenter.ContentTemplate = instance.PgcTemplate;
                    }
                    else if (mainModule.Type == Bilibili.App.Dynamic.V2.ModuleDynamicType.MdlDynArchive)
                    {
                        instance.AddViewLaterButton.IsEnabled = !mainModule.DynArchive.IsPGC;
                        instance.MainContentPresenter.Content = new VideoViewModel(mainModule.DynArchive);
                        instance.MainContentPresenter.ContentTemplate = instance.VideoTemplate;
                    }
                }

                if (dataModule != null)
                {
                    var numberToolkit = ServiceLocator.Instance.GetService<INumberToolkit>();
                    instance.LikeCountBlock.Text = numberToolkit.GetCountText(dataModule.Like);
                    instance.ReplyCountBlock.Text = dataModule.Reply.ToString();
                    instance.LikeButton.IsChecked = dataModule.LikeInfo.IsLike;
                }
            }
        }

        private void OnCardClick(object sender, RoutedEventArgs e)
        {
            OpenPlayer();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckOrientation();
        }

        private void CheckOrientation()
        {
            var child = VisualTreeHelper.GetChild(MainContentPresenter, 0);
            if (child is IDynamicLayoutItem item)
            {
                item.Orientation = Orientation;
            }
        }

        private void OnReplyButtonClick(object sender, RoutedEventArgs e)
        {
            OpenPlayer();
            PlayerViewModel.Instance.InitializeSection = AppConstants.ReplySection;
        }

        private void OpenPlayer()
        {
            if (MainContentPresenter.Content is VideoViewModel videoVM)
            {
                AppViewModel.Instance.OpenPlayer(videoVM);
            }
            else if (MainContentPresenter.Content is SeasonViewModel seasonVM)
            {
                AppViewModel.Instance.OpenPlayer(seasonVM);
            }
        }

        private async void OnLikeButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var dataModule = Data.Modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleStat).FirstOrDefault()?.ModuleStat;
            var isLike = dataModule?.LikeInfo?.IsLike ?? false;
            LikeButton.IsChecked = isLike;

            LikeButton.IsEnabled = false;
            var result = await DynamicModuleViewModel.Instance.LikeDynamicAsync(Data, !isLike);
            if (result)
            {
                LikeButton.IsChecked = !isLike;
                dataModule.Like = !isLike ? dataModule.Like + 1 : dataModule.Like - 1;
                dataModule.LikeInfo.IsLike = !isLike;
                var numberToolkit = ServiceLocator.Instance.GetService<INumberToolkit>();
                LikeCountBlock.Text = numberToolkit.GetCountText(dataModule.Like);
            }

            LikeButton.IsEnabled = true;
        }

        private async void OnAddToViewLaterItemClickAsync(object sender, RoutedEventArgs e)
        {
            if (MainContentPresenter.Content is VideoViewModel videoVM)
            {
                await ViewLaterViewModel.Instance.AddAsync(videoVM);
            }
        }
    }
}
