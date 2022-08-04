// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using Splat;

namespace Bili.App.Controls.Base
{
    /// <summary>
    /// 导航框架的基类.
    /// </summary>
    public class NavigationViewBase : ReactiveUserControl<INavigationViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewBase"/> class.
        /// </summary>
        public NavigationViewBase()
        {
            ViewModel = Locator.Current.GetService<INavigationViewModel>();
            AppViewModel = Locator.Current.GetService<IAppViewModel>();
            AccountViewModel = Locator.Current.GetService<IAccountViewModel>();
        }

        /// <summary>
        /// 在刚加载首页时发生.
        /// </summary>
        public event EventHandler FirstLoaded;

        /// <summary>
        /// 应用视图模型.
        /// </summary>
        protected IAppViewModel AppViewModel { get; }

        /// <summary>
        /// 账户视图模型.
        /// </summary>
        protected IAccountViewModel AccountViewModel { get; }

        /// <summary>
        /// 是否为初次加载.
        /// </summary>
        protected bool IsFirstLoaded { get; set; }

        /// <summary>
        /// 处理固定条目点击.
        /// </summary>
        /// <param name="item">固定条目.</param>
        protected void HandleFixItemClicked(FixedItem item)
        {
            PlaySnapshot playRecord = null;
            switch (item.Type)
            {
                case Models.Enums.App.FixedType.Publisher:
                    ViewModel.NavigateToSecondaryView(PageIds.UserSpace, item.Id);
                    break;
                case Models.Enums.App.FixedType.Pgc:
                    playRecord = new PlaySnapshot(default, item.Id, VideoType.Pgc)
                    {
                        Title = item.Title,
                    };
                    break;
                case Models.Enums.App.FixedType.Video:
                    playRecord = new PlaySnapshot(item.Id, default, VideoType.Video);
                    break;

                case Models.Enums.App.FixedType.Live:
                    playRecord = new PlaySnapshot(item.Id, default, VideoType.Live);
                    break;
                default:
                    break;
            }

            if (playRecord != null)
            {
                ViewModel.NavigateToPlayView(playRecord);
            }
        }

        /// <summary>
        /// 触发首次加载事件.
        /// </summary>
        protected void FireFirstLoadedEvent()
            => FirstLoaded?.Invoke(this, EventArgs.Empty);
    }
}
