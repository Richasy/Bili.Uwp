// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.Locator.Uwp;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
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

        private readonly NavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItem"/> class.
        /// </summary>
        public DynamicItem()
        {
            InitializeComponent();
            _navigationViewModel = Splat.Locator.Current.GetService<NavigationViewModel>();
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
            return Orientation == Orientation.Horizontal
                ? new Size(double.PositiveInfinity, 180)
                : new Size(320, 248);
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
                    instance.UserAvatar.DataContext = new UserViewModel(userModule.Author.Name, userModule.Author.Face, Convert.ToInt32(userModule.Mid));
                }

                if (descModule != null)
                {
                    instance.MainContainer.Margin = string.IsNullOrEmpty(descModule.Text.Trim())
                        ? new Thickness(0, -40, 0, 0)
                        : new Thickness(0, -4, 0, 0);
                    instance.DescriptionBlock.DynamicDescription = descModule;
                }
                else
                {
                    instance.MainContainer.Margin = new Thickness(0, -40, 0, 0);
                    instance.DescriptionBlock.Reset();
                }

                instance.Presenter.Data = data;

                if (dataModule != null)
                {
                    var numberToolkit = ServiceLocator.Instance.GetService<INumberToolkit>();
                    instance.LikeCountBlock.Text = numberToolkit.GetCountText(dataModule.Like);
                    instance.ReplyCountBlock.Text = dataModule.Reply.ToString();
                    instance.LikeButton.IsChecked = dataModule.LikeInfo.IsLike;
                }
            }
        }

        private async void OnCardClickAsync(object sender, RoutedEventArgs e)
            => await JumpAsync();

        private void OnLoaded(object sender, RoutedEventArgs e)
            => CheckOrientation();

        private void CheckOrientation()
        {
            Presenter.ChangeOrientation(Orientation);
        }

        private async void OnReplyButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var type = ReplyType.None;
            switch (Data.CardType)
            {
                case Bilibili.App.Dynamic.V2.DynamicType.Forward:
                case Bilibili.App.Dynamic.V2.DynamicType.Word:
                case Bilibili.App.Dynamic.V2.DynamicType.Live:
                    type = ReplyType.Dynamic;
                    break;
                case Bilibili.App.Dynamic.V2.DynamicType.Draw:
                    type = ReplyType.Album;
                    break;
                case Bilibili.App.Dynamic.V2.DynamicType.Av:
                case Bilibili.App.Dynamic.V2.DynamicType.Pgc:
                case Bilibili.App.Dynamic.V2.DynamicType.UgcSeason:
                case Bilibili.App.Dynamic.V2.DynamicType.Medialist:
                    type = ReplyType.Video;
                    break;
                case Bilibili.App.Dynamic.V2.DynamicType.Courses:
                case Bilibili.App.Dynamic.V2.DynamicType.CoursesSeason:
                    type = ReplyType.Course;
                    break;
                case Bilibili.App.Dynamic.V2.DynamicType.Article:
                    type = ReplyType.Article;
                    break;
                case Bilibili.App.Dynamic.V2.DynamicType.Music:
                    type = ReplyType.Music;
                    break;
                default:
                    break;
            }

            if (type == ReplyType.None)
            {
                var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
                Splat.Locator.Current.GetService<AppViewModel>().ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.NotSupportReplyType), Models.Enums.App.InfoType.Warning);
                return;
            }

            var id = type == ReplyType.Dynamic ? Data.Extend.DynIdStr : Data.Extend.BusinessId;
            ReplyModuleViewModel.Instance.SetInformation(Convert.ToInt64(id), type, Bilibili.Main.Community.Reply.V1.Mode.MainListTime);
            await ReplyDetailView.Instance.ShowAsync(ReplyModuleViewModel.Instance);
        }

        private async Task JumpAsync()
        {
            var vm = Presenter.GetPlayViewModel();
            if (vm != null)
            {
                _navigationViewModel.NavigateToPlayView(vm);
                return;
            }

            vm = Presenter.GetArticleViewModel();
            if (vm != null)
            {
                // await ArticleReaderView.Instance.ShowAsync(vm as ArticleViewModel);
                await Task.CompletedTask;
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
            var vm = Presenter.GetPlayViewModel();
            if (vm is VideoViewModel videoVM)
            {
                // await ViewLaterViewModel.Instance.AddAsync(videoVM);
                await Task.CompletedTask;
            }
        }

        private void OnShareButtonClick(object sender, RoutedEventArgs e)
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += OnDataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            var url = string.Empty;
            var coverUrl = string.Empty;
            var title = string.Empty;
            var mainModule = Data.Modules.ToList().Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleDynamic).FirstOrDefault()?.ModuleDynamic;
            var descModule = Data.Modules.ToList().Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleDesc).FirstOrDefault();
            if (mainModule != null)
            {
                request.Data.SetText(descModule?.ModuleDesc?.Text ?? string.Empty);
                if (mainModule.ModuleItemCase == Bilibili.App.Dynamic.V2.ModuleDynamic.ModuleItemOneofCase.DynPgc)
                {
                    var pgc = mainModule.DynPgc;
                    url = $"https://www.bilibili.com/bangumi/play/ss{pgc.SeasonId}/";
                    title = pgc.Title;
                    coverUrl = pgc.Cover;
                }
                else if (mainModule.ModuleItemCase == Bilibili.App.Dynamic.V2.ModuleDynamic.ModuleItemOneofCase.DynArchive)
                {
                    var archive = mainModule.DynArchive;
                    url = archive.IsPGC ?
                        $"https://www.bilibili.com/bangumi/play/ss{archive.PgcSeasonId}/" :
                        $"https://www.bilibili.com/video/{archive.Bvid}";
                    title = archive.Title;
                    coverUrl = archive.Cover;
                }
            }

            request.Data.Properties.Title = title;
            if (!string.IsNullOrEmpty(url))
            {
                request.Data.SetWebLink(new Uri(url));
            }

            if (!string.IsNullOrEmpty(coverUrl))
            {
                request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(coverUrl)));
            }
        }

        private async void OnAvatarClickAsync(object sender, EventArgs e)
        {
            if ((sender as FrameworkElement).DataContext is UserViewModel data)
            {
                await UserView.Instance.ShowAsync(data);
            }
        }

        private void OnMoreFlyoutOpened(object sender, object e)
        {
            var vm = Presenter.GetPlayViewModel();
            AddViewLaterButton.IsEnabled = vm is VideoViewModel;
        }
    }
}
