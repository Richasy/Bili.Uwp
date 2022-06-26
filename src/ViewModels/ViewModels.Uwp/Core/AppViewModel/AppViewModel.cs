// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.App.Constants;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Pgc;
using ReactiveUI;
using Splat;
using Windows.Globalization;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 应用ViewModel.
    /// </summary>
    public sealed partial class AppViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppViewModel"/> class.
        /// </summary>
        public AppViewModel()
        {
            _navigationViewModel = Locator.Current.GetService<NavigationViewModel>();
            _resourceToolkit = Locator.Current.GetService<IResourceToolkit>();
            _settingsToolkit = Locator.Current.GetService<ISettingsToolkit>();
            _fileToolkit = Locator.Current.GetService<IFileToolkit>();
            _appToolkit = Locator.Current.GetService<IAppToolkit>();
            _updateProvider = Locator.Current.GetService<IUpdateProvider>();
            _networkHelper = Microsoft.Toolkit.Uwp.Connectivity.NetworkHelper.Instance;
            IsXbox = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";
            IsNavigatePaneOpen = !IsXbox;
            _isWide = null;
            _networkHelper.NetworkChanged += OnNetworkChanged;
            IsNetworkAvaliable = _networkHelper.ConnectionInformation.IsInternetAvailable;
            IsShowTitleBar = true;
            IsShowPlayRecordButton = false;
            PlayRecords = new ObservableCollection<PlayRecord>();

            CheckUpdateCommand = ReactiveCommand.CreateFromTask(CheckUpdateAsync, outputScheduler: RxApp.MainThreadScheduler);
            CheckContinuePlayCommand = ReactiveCommand.Create(CheckContinuePlay, outputScheduler: RxApp.MainThreadScheduler);
            CheckNewDynamicRegistrationCommand = ReactiveCommand.CreateFromTask(CheckNewDynamicRegistrationAsync, outputScheduler: RxApp.MainThreadScheduler);
            AddLastPlayItemCommand = ReactiveCommand.CreateFromTask<PlaySnapshot>(AddLastPlayItemAsync, outputScheduler: RxApp.MainThreadScheduler);
            DeleteLastPlayItemCommand = ReactiveCommand.CreateFromTask(DeleteLastPlayItemAsync, outputScheduler: RxApp.MainThreadScheduler);
            AddPlayRecordCommand = ReactiveCommand.Create<PlayRecord>(AddPlayRecord, outputScheduler: RxApp.MainThreadScheduler);
            RemovePlayRecordCommand = ReactiveCommand.Create<PlayRecord>(RemovePlayRecord, outputScheduler: RxApp.MainThreadScheduler);
            ClearPlayRecordCommand = ReactiveCommand.Create(ClearPlayRecords, outputScheduler: RxApp.MainThreadScheduler);

            CheckUpdateCommand.ThrownExceptions.Subscribe(LogException);
            PlayRecords.CollectionChanged += OnPlayRecordsCollectionChanged;

            RxApp.DefaultExceptionHandler = new UnhandledExceptionHandler();

            var lan = ApplicationLanguages.Languages.First();
            _settingsToolkit.WriteLocalSetting(SettingNames.LastAppLanguage, lan);
            IsTraditionalChinese = lan.Contains("zh-hant", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 显示提示.
        /// </summary>
        /// <param name="message">消息内容.</param>
        /// <param name="type">消息类型.</param>
        public void ShowTip(string message, InfoType type = InfoType.Information)
            => RequestShowTip?.Invoke(this, new AppTipNotificationEventArgs(message, type));

        /// <summary>
        /// 显示图片.
        /// </summary>
        /// <param name="images">图片列表.</param>
        /// <param name="firstIndex">初始索引.</param>
        public void ShowImages(IEnumerable<Models.Data.Appearance.Image> images, int firstIndex)
        {
            if (images == null)
            {
                RequestShowImages?.Invoke(this, null);
            }
            else
            {
                RequestShowImages?.Invoke(this, new ShowImageEventArgs(images, firstIndex));
            }
        }

        /// <summary>
        /// 显示 PGC 播放列表.
        /// </summary>
        /// <param name="vm">播放列表视图模型.</param>
        public void ShowPgcPlaylist(PgcPlaylistViewModel vm)
            => RequestShowPgcPlaylist?.Invoke(this, vm);

        /// <summary>
        /// 显示文章阅读器.
        /// </summary>
        /// <param name="article">文章信息.</param>
        public void ShowArticleReader(ArticleItemViewModel article)
            => RequestShowArticleReader?.Invoke(this, article);

        /// <summary>
        /// 显示评论详情.
        /// </summary>
        /// <param name="args">评论信息.</param>
        public void ShowReply(ShowCommentEventArgs args)
            => RequestShowReplyDetail?.Invoke(this, args);

        /// <summary>
        /// 显示用户详情.
        /// </summary>
        /// <param name="vm">用户条目视图模型.</param>
        public void ShowUserDetail(UserItemViewModel vm)
            => RequestShowUserDetail?.Invoke(this, vm);

        /// <summary>
        /// 显示视频收藏夹详情.
        /// </summary>
        /// <param name="folder">视频收藏夹.</param>
        public void ShowVideoFavoriteFolderDetail(VideoFavoriteFolder folder)
            => RequestShowVideoFavoriteFolderDetail.Invoke(this, folder);

        /// <summary>
        /// 显示正在播放的剧集信息详情.
        /// </summary>
        public void ShowPgcSeasonDetail()
            => RequestShowPgcSeasonDetail.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// 初始化主题.
        /// </summary>
        public void InitializeTheme()
        {
            var theme = _settingsToolkit.ReadLocalSetting(SettingNames.AppTheme, AppConstants.ThemeDefault);
            Theme = theme switch
            {
                AppConstants.ThemeLight => ElementTheme.Light,
                AppConstants.ThemeDark => ElementTheme.Dark,
                _ => ElementTheme.Default
            };
        }

        /// <summary>
        /// 初始化边距设置.
        /// </summary>
        public void InitializePadding()
        {
            var width = Window.Current.Bounds.Width;
            if (IsXbox)
            {
                PageHorizontalPadding = _resourceToolkit.GetResource<Thickness>("XboxPageHorizontalPadding");
                PageTopPadding = _resourceToolkit.GetResource<Thickness>("XboxPageTopPadding");
            }
            else
            {
                var isWide = _isWide.HasValue && _isWide.Value;
                if (width >= MediumWindowThresholdWidth)
                {
                    if (!isWide)
                    {
                        _isWide = true;
                        PageHorizontalPadding = _resourceToolkit.GetResource<Thickness>("DefaultPageHorizontalPadding");
                        PageTopPadding = _resourceToolkit.GetResource<Thickness>("DefaultPageTopPadding");
                    }
                }
                else
                {
                    _isWide = false;
                    PageHorizontalPadding = _resourceToolkit.GetResource<Thickness>("NarrowPageHorizontalPadding");
                    PageTopPadding = _resourceToolkit.GetResource<Thickness>("NarrowPageTopPadding");
                }
            }
        }

        /// <summary>
        /// 获取上一次播放的条目.
        /// </summary>
        /// <returns><see cref="PlaySnapshot"/>.</returns>
        public Task<PlaySnapshot> GetLastPlayItemAsync()
            => _fileToolkit.ReadLocalDataAsync<PlaySnapshot>(AppConstants.LastOpenVideoFileName);
    }
}
