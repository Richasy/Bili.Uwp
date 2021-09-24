// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.App;

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
            _ = BiliController.Instance;
            IsNavigatePaneOpen = true;
            IsBackButtonEnabled = true;
            CurrentMainContentId = PageIds.Recommend;
            ServiceLocator.Instance.LoadService(out _resourceToolkit)
                                   .LoadService(out _settingToolkit);
            _displayRequest = new Windows.System.Display.DisplayRequest();
            InitializeTheme();
        }

        /// <summary>
        /// 显示提示.
        /// </summary>
        /// <param name="message">消息内容.</param>
        /// <param name="type">消息类型.</param>
        public void ShowTip(string message, InfoType type = InfoType.Information)
        {
            RequestShowTip?.Invoke(this, new Models.App.Args.AppTipNotificationEventArgs(message, type));
        }

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
        /// 激活显示请求.
        /// </summary>
        public void ActiveDisplayRequest()
        {
            _displayRequest.RequestActive();
        }

        /// <summary>
        /// 释放显示请求.
        /// </summary>
        public void ReleaseDisplayRequest()
        {
            _displayRequest.RequestRelease();
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
                    Theme = Windows.UI.Xaml.ElementTheme.Light;
                    break;
                case AppConstants.ThemeDark:
                    Theme = Windows.UI.Xaml.ElementTheme.Dark;
                    break;
                case AppConstants.ThemeDefault:
                    Theme = Windows.UI.Xaml.ElementTheme.Default;
                    break;
                default:
                    break;
            }
        }
    }
}
