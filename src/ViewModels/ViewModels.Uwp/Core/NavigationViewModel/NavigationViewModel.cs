﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Bili.Models.App.Args;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using ReactiveUI;
using Splat;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 处理导航的视图模型.
    /// </summary>
    public sealed partial class NavigationViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewModel"/> class.
        /// </summary>
        public NavigationViewModel()
        {
            _backStack = new List<AppBackEventArgs>();

            IsMainViewShown = true;
            var canBack = this.WhenAnyValue(x => x.CanBack);
            BackCommand = ReactiveCommand.Create(Back, canBack, RxApp.MainThreadScheduler);

            this.WhenAnyValue(x => x.IsMainViewShown)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        IsPlayViewShown = IsSecondaryViewShown = false;
                    }
                });

            this.WhenAnyValue(x => x.IsSecondaryViewShown)
               .Subscribe(x =>
               {
                   if (x)
                   {
                       IsPlayViewShown = IsMainViewShown = false;
                   }
               });

            this.WhenAnyValue(x => x.IsPlayViewShown)
                .Subscribe(x =>
                {
                    if (x)
                    {
                        IsMainViewShown = IsSecondaryViewShown = false;
                    }
                });
        }

        /// <summary>
        /// 传入页面 Id, 并由应用自行判断层级，导航到指定类型的页面.
        /// </summary>
        /// <param name="pageId">页面 Id.</param>
        /// <param name="parameter">导航参数.</param>
        public void Navigate(PageIds pageId, object parameter = null)
        {
            var type = pageId.GetHashCode() switch
            {
                < 100 => NavigationType.Main,
                < 1000 => NavigationType.Secondary,
                _ => NavigationType.Player,
            };

            switch (type)
            {
                case NavigationType.Main:
                    NavigateToMainView(pageId, parameter);
                    break;
                case NavigationType.Secondary:
                    NavigateToSecondaryView(pageId, parameter);
                    break;
                case NavigationType.Player:
                    NavigateToPlayView((PlaySnapshot)parameter);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 在主视图中进行导航，传入的 PageIds 应该是主视图的页面 Id.
        /// </summary>
        /// <param name="pageId">页面 Id.</param>
        /// <param name="parameter">导航参数.</param>
        public void NavigateToMainView(PageIds pageId, object parameter = null)
        {
            IsMainViewShown = true;
            SecondaryViewId = PageIds.None;
            PlayViewId = PageIds.None;
            CloseAllPopup();
            RemoveAllPlayer();
            if (pageId != MainViewId)
            {
                RemoveBackStack(BackBehavior.MainView);
                MainViewId = pageId;
                var args = new AppNavigationEventArgs(NavigationType.Main, pageId, parameter);
                AddBackStack(BackBehavior.MainView, null, pageId);
                Navigating?.Invoke(this, args);
            }
        }

        /// <summary>
        /// 导航到指定的二级页面，传入的 PageIds 应该是二级页面的页面 Id.
        /// </summary>
        /// <param name="pageId">页面 Id.</param>
        /// <param name="parameter">导航参数.</param>
        public void NavigateToSecondaryView(PageIds pageId, object parameter = null)
        {
            IsSecondaryViewShown = true;
            PlayViewId = PageIds.None;
            CloseAllPopup();
            RemoveAllPlayer();
            if (pageId != SecondaryViewId || pageId == PageIds.Search)
            {
                SecondaryViewId = pageId;
                var args = new AppNavigationEventArgs(NavigationType.Secondary, pageId, parameter);
                AddBackStack(
                    BackBehavior.SecondaryView,
                    _ => TryLayerBack(),
                    pageId);
                Navigating?.Invoke(this, args);
            }
        }

        /// <summary>
        /// 导航到播放页，传入播放参数.
        /// </summary>
        /// <param name="parameter">播放参数.</param>
        public void NavigateToPlayView(PlaySnapshot parameter)
        {
            IsPlayViewShown = true;
            CloseAllPopup();
            RemoveBackStack(BackBehavior.OpenPlayer);
            var pageId = GetPageIdFromPlaySnapshot(parameter);
            PlayViewId = pageId;
            var args = new AppNavigationEventArgs(NavigationType.Player, pageId, parameter);
            AddBackStack(
                    BackBehavior.OpenPlayer,
                    _ =>
                    {
                        TryLayerBack();
                        var last = _backStack.Last();
                        if (last.Id != BackBehavior.OpenPlayer)
                        {
                            ExitPlayer?.Invoke(this, EventArgs.Empty);
                        }
                    },
                    null);
            Navigating?.Invoke(this, args);
        }

        /// <summary>
        /// 导航到播放页，传入播放参数.
        /// </summary>
        /// <param name="parameters">播放参数.</param>
        public void NavigateToPlayView(IEnumerable<VideoInformation> parameters, int startIndex = 0)
        {
            IsPlayViewShown = true;
            CloseAllPopup();
            RemoveBackStack(BackBehavior.OpenPlayer);
            var pageId = PageIds.VideoPlayer;
            PlayViewId = pageId;
            var args = new AppNavigationEventArgs(NavigationType.Player, pageId, new Tuple<IEnumerable<VideoInformation>, int>(parameters, startIndex));
            AddBackStack(
                    BackBehavior.OpenPlayer,
                    _ =>
                    {
                        TryLayerBack();
                        var last = _backStack.Last();
                        if (last.Id != BackBehavior.OpenPlayer)
                        {
                            ExitPlayer?.Invoke(this, EventArgs.Empty);
                        }
                    },
                    null);
            Navigating?.Invoke(this, args);
        }

        /// <summary>
        /// 添加后退栈，应用会按照先进后出的顺序依次执行回退行为.
        /// </summary>
        /// <param name="id">行为标识.</param>
        /// <param name="backBehavior">自定义的后退行为.</param>
        /// <param name="parameter">后退时携带的参数.</param>
        /// <remarks>
        /// 这里的导航服务的主要职能是暂存和转发。应用各个组件定义自己的后退行为，按照顺序依次进入这个栈中，
        /// 导航服务不必知道 <see cref="Action"/> 是做什么的，只负责在需要后退时按顺序执行这些组件自定义的行为即可.
        /// 举例来说，当应用进入二级页面，由组件发送一个回退行为给导航服务，当应用点击标题栏的后退按钮时，
        /// 服务检测到最后一个回退行为是二级页面定义的，那么就执行二级页面预设的 <see cref="Action"/>.
        /// </remarks>
        public void AddBackStack(BackBehavior id, Action<object> backBehavior, object parameter = null)
        {
            var args = new AppBackEventArgs(id, backBehavior, parameter);
            _backStack.RemoveAll(p => p.Equals(args));
            _backStack.Add(args);
            CheckBackStatus();
        }

        /// <summary>
        /// 移除已有的后退栈.
        /// </summary>
        /// <param name="id">行为标识.</param>
        public void RemoveBackStack(BackBehavior id)
        {
            if (_backStack.Any(p => p.Id == id))
            {
                _backStack.Remove(_backStack.Last(p => p.Id == id));
            }

            CheckBackStatus();
        }

        private void Back()
        {
            if (!CanBack)
            {
                return;
            }

            var last = _backStack.Last();
            RemoveBackStack(last.Id);
            last.Action?.Invoke(last.Parameter);
        }

        private void TryLayerBack()
        {
            if (_backStack.Count == 0)
            {
                return;
            }

            var last = _backStack.Last();
            if (last.Id == BackBehavior.MainView)
            {
                NavigateToMainView((PageIds)last.Parameter, null);
                Splat.Locator.Current.GetService<AppViewModel>().DeleteLastPlayItemCommand.Execute().Subscribe();
            }
            else if (last.Id == BackBehavior.SecondaryView)
            {
                NavigateToSecondaryView((PageIds)last.Parameter, null);
                Splat.Locator.Current.GetService<AppViewModel>().DeleteLastPlayItemCommand.Execute().Subscribe();
            }
            else if (last.Id == BackBehavior.OpenPlayer)
            {
                if (last.Parameter is PlaySnapshot shot)
                {
                    NavigateToPlayView(shot);
                }
            }
        }

        private void CloseAllPopup()
        {
            _backStack.Where(p => p.Id == BackBehavior.ShowHolder)
                .ToList()
                .ForEach(p => p.Action?.Invoke(p.Parameter));
            _backStack.RemoveAll(p => p.Id == BackBehavior.ShowHolder);
        }

        private void RemoveAllPlayer()
        {
            _backStack.Where(p => p.Id == BackBehavior.OpenPlayer)
                .ToList()
                .ForEach(p => p.Action?.Invoke(p.Parameter));
            _backStack.RemoveAll(p => p.Id == BackBehavior.OpenPlayer);
        }

        private void CheckBackStatus()
        {
            CanBack = _backStack.Count > 1;
            IsBackButtonEnabled = CanBack
                && _backStack.Last().Id != BackBehavior.ShowHolder
                && _backStack.Last().Id != BackBehavior.PlayerModeChange;
        }

        private PageIds GetPageIdFromPlaySnapshot(PlaySnapshot shot)
        {
            var pageId = shot.VideoType switch
            {
                VideoType.Video => PageIds.VideoPlayer,
                VideoType.Pgc => PageIds.PgcPlayer,
                _ => PageIds.LivePlayer,
            };

            return pageId;
        }
    }
}
