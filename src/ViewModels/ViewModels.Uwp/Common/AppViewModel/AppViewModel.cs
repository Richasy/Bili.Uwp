// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.App;
using Windows.UI.Xaml;

namespace Richasy.Bili.ViewModels.Uwp
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
            IsBackButtonEnabled = true;
            CurrentMainContentId = PageIds.Recommend;
            ServiceLocator.Instance.LoadService(out _resourceToolkit)
                                   .LoadService(out _settingToolkit)
                                   .LoadService(out _loggerModule);
            _displayRequest = new Windows.System.Display.DisplayRequest();
            IsXbox = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Xbox";
            IsNavigatePaneOpen = !IsXbox;
            _isWide = null;
            _controller.UpdateReceived += OnUpdateReceived;
            InitializeTheme();
        }

        /// <summary>
        /// 返回.
        /// </summary>
        public void Back()
            => RequestBack?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// 显示提示.
        /// </summary>
        /// <param name="message">消息内容.</param>
        /// <param name="type">消息类型.</param>
        public void ShowTip(string message, InfoType type = InfoType.Information)
            => RequestShowTip?.Invoke(this, new AppTipNotificationEventArgs(message, type));

        /// <summary>
        /// 修改当前主内容标识.
        /// </summary>
        /// <param name="pageId">主内容标识.</param>
        public void SetMainContentId(PageIds pageId)
        {
            CurrentMainContentId = pageId;
            CurrentOverlayContentId = PageIds.None;
            IsShowOverlay = false;
        }

        /// <summary>
        /// 修改当前覆盖内容标识.
        /// </summary>
        /// <param name="pageId">覆盖内容标识.</param>
        /// <param name="param">导航参数.</param>
        public void SetOverlayContentId(PageIds pageId, object param = null)
        {
            CurrentOverlayContentId = pageId;
            IsShowOverlay = true;
            IsOpenPlayer = false;
            RequestOverlayNavigation?.Invoke(this, param);
        }

        /// <summary>
        /// 打开播放器播放视频.
        /// </summary>
        /// <param name="playVM">包含播放信息的视图模型.</param>
        public void OpenPlayer(object playVM)
        {
            RequestPlay?.Invoke(this, playVM);
            IsOpenPlayer = true;
        }

        /// <summary>
        /// 显示图片.
        /// </summary>
        /// <param name="images">图片列表.</param>
        /// <param name="firstIndex">初始索引.</param>
        public void ShowImages(List<string> images, int firstIndex)
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
        /// 激活显示请求.
        /// </summary>
        public void ActiveDisplayRequest()
        {
            if (_displayRequest != null)
            {
                try
                {
                    _displayRequest.RequestActive();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 释放显示请求.
        /// </summary>
        public void ReleaseDisplayRequest()
        {
            if (_displayRequest != null)
            {
                try
                {
                    _displayRequest.RequestRelease();
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// 进入相关用户视图.
        /// </summary>
        /// <param name="type">粉丝视图或关注视图.</param>
        /// <param name="userId">用户Id.</param>
        /// <param name="userName">用户名.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task EnterRelatedUserViewAsync(RelatedUserType type, int userId, string userName)
        {
            Enum.TryParse<PageIds>(type.ToString(), out var targetPageId);
            if (CurrentOverlayContentId != targetPageId)
            {
                SetOverlayContentId(targetPageId, new Tuple<int, string>(userId, userName));
            }
            else
            {
                var targetVM = type == RelatedUserType.Fans ? FansViewModel.Instance : (RelatedUserViewModel)FollowsViewModel.Instance;
                var canRefresh = targetVM.SetUser(userId, userName);
                if (canRefresh)
                {
                    await targetVM.InitializeRequestAsync();
                }
            }
        }

        /// <summary>
        /// 初始化主题.
        /// </summary>
        public void InitializeTheme()
        {
            var theme = _settingToolkit.ReadLocalSetting(SettingNames.AppTheme, AppConstants.ThemeDefault);
            switch (theme)
            {
                case AppConstants.ThemeLight:
                    Theme = ElementTheme.Light;
                    break;
                case AppConstants.ThemeDark:
                    Theme = ElementTheme.Dark;
                    break;
                case AppConstants.ThemeDefault:
                    Theme = ElementTheme.Default;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初始化边距设置.
        /// </summary>
        public void InitializePadding()
        {
            var width = Window.Current.Bounds.Width;
            if (IsXbox)
            {
                PageLeftPadding = _resourceToolkit.GetResource<Thickness>("XboxPagePadding");
                PageRightPadding = _resourceToolkit.GetResource<Thickness>("XboxContainerPadding");
            }
            else
            {
                var isWide = _isWide.HasValue && _isWide.Value;
                if (width >= MediumWindowThresholdWidth)
                {
                    if (!isWide)
                    {
                        _isWide = true;
                        PageLeftPadding = _resourceToolkit.GetResource<Thickness>("DefaultPagePadding");
                        PageRightPadding = _resourceToolkit.GetResource<Thickness>("DefaultContainerPadding");
                    }
                }
                else
                {
                    _isWide = false;
                    PageLeftPadding = _resourceToolkit.GetResource<Thickness>("NarrowPagePadding");
                    PageRightPadding = _resourceToolkit.GetResource<Thickness>("NarrowContainerPadding");
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

        private void OnUpdateReceived(object sender, UpdateEventArgs e) => RequestShowUpdateDialog?.Invoke(this, e);
    }
}
