// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bili.Controller.Uwp;
using Bili.Models.App.Args;
using Bili.Models.App.Constants;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Pgc;
using Splat;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 应用ViewModel.
    /// </summary>
    public partial class AppViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppViewModel"/> class.
        /// </summary>
        internal AppViewModel()
        {
            _controller = BiliController.Instance;
            _navigationViewModel = Splat.Locator.Current.GetService<NavigationViewModel>();
            _resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
            _settingToolkit = Splat.Locator.Current.GetService<ISettingsToolkit>();
            _networkHelper = Microsoft.Toolkit.Uwp.Connectivity.NetworkHelper.Instance;
            IsXbox = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";
            IsNavigatePaneOpen = !IsXbox;
            _isWide = null;
            _controller.UpdateReceived += OnUpdateReceived;
            _networkHelper.NetworkChanged += OnNetworkChanged;
            IsNetworkAvaliable = _networkHelper.ConnectionInformation.IsInternetAvailable;
            InitializeTheme();
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
        /// <param name="information">评论信息.</param>
        public void ShowReply(MessageInformation information)
            => RequestShowReplyDetail?.Invoke(this, information);

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
        /// 初始化主题.
        /// </summary>
        public void InitializeTheme()
        {
            var theme = _settingToolkit.ReadLocalSetting(SettingNames.AppTheme, AppConstants.ThemeDefault);
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
        /// 检查更新.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public Task CheckUpdateAsync()
            => _controller.CheckUpdateAsync();

        /// <summary>
        /// 检查是否可以继续播放.
        /// </summary>
        public void CheckContinuePlay()
        {
            var supportCheck = _settingToolkit.ReadLocalSetting(SettingNames.SupportContinuePlay, true);
            var canPlay = _settingToolkit.ReadLocalSetting(SettingNames.CanContinuePlay, false);
            if (supportCheck && canPlay)
            {
                RequestContinuePlay?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 注册新动态通知的后台通知任务.
        /// </summary>
        /// <returns>注册结果.</returns>
        public async Task<bool> RegisterNewDynamicBackgroundTaskAsync()
        {
            var taskName = AppConstants.NewDynamicTaskName;
            var hasRegistered = BackgroundTaskRegistration.AllTasks.Any(p => p.Value.Name.Equals(taskName));
            if (!hasRegistered)
            {
                var status = await BackgroundExecutionManager.RequestAccessAsync();
                if (!status.ToString().Contains("Allowed"))
                {
                    return false;
                }

                var builder = new BackgroundTaskBuilder
                {
                    Name = taskName,
                    TaskEntryPoint = taskName,
                };
                builder.SetTrigger(new TimeTrigger(15, false));
                builder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));
                _ = builder.Register();
            }

            return true;
        }

        /// <summary>
        /// 注销新动态通知任务.
        /// </summary>
        public void UnregisterNewDynamicBackgroundTask()
        {
            var taskName = AppConstants.NewDynamicTaskName;
            var task = BackgroundTaskRegistration.AllTasks.FirstOrDefault(p => p.Value.Name.Equals(taskName)).Value;
            if (task != null)
            {
                task.Unregister(true);
            }
        }

        /// <summary>
        /// 检查新动态通知是否启用.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task CheckNewDynamicRegistrationAsync()
        {
            var openDynamicNotify = _settingToolkit.ReadLocalSetting(SettingNames.IsOpenNewDynamicNotify, true);
            if (openDynamicNotify)
            {
                await RegisterNewDynamicBackgroundTaskAsync();
            }
            else
            {
                UnregisterNewDynamicBackgroundTask();
            }
        }

        private void OnUpdateReceived(object sender, UpdateEventArgs e)
            => RequestShowUpdateDialog?.Invoke(this, e);

        private void OnNetworkChanged(object sender, EventArgs e)
            => IsNetworkAvaliable = _networkHelper.ConnectionInformation.IsInternetAvailable;
    }
}
