// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.ViewModels.Interfaces;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 处理导航的视图模型.
    /// </summary>
    public sealed partial class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewModel"/> class.
        /// </summary>
        public NavigationViewModel()
        {
            _backStack = new List<AppBackEventArgs>();

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

        /// <inheritdoc/>
        public void Navigate(PageIds pageId, object parameter)
        {
            var type = pageId.GetHashCode() switch
            {
                < 100 => NavigationType.Main,
                < 1000 => NavigationType.Secondary,
                1001 => NavigationType.Player,
                _ => throw new ArgumentOutOfRangeException(nameof(pageId))
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
                    NavigateToPlayView(parameter);
                    break;
                default:
                    break;
            }
        }

        /// <inheritdoc/>
        public void NavigateToMainView(PageIds pageId, object parameter = null)
        {
            if (pageId != MainViewId)
            {
                RemoveBackStack(BackBehavior.MainView);
                MainViewId = pageId;
                var args = new AppNavigationEventArgs(NavigationType.Main, pageId, parameter);
                AddBackStack(BackBehavior.MainView, null, pageId);
                Navigating?.Invoke(this, args);
            }

            IsMainViewShown = true;
        }

        /// <inheritdoc/>
        public void NavigateToPlayView(object parameter)
        {
            RemoveBackStack(BackBehavior.OpenPlayer);
            var args = new AppNavigationEventArgs(NavigationType.Player, PageIds.Player, parameter);
            IsPlayViewShown = true;
            AddBackStack(BackBehavior.OpenPlayer, _ => TryLayerBack(), parameter);
            Navigating?.Invoke(this, args);
        }

        /// <inheritdoc/>
        public void NavigateToSecondaryView(PageIds pageId, object parameter = null)
        {
            if (pageId != SecondaryViewId)
            {
                SecondaryViewId = pageId;
                var args = new AppNavigationEventArgs(NavigationType.Secondary, pageId, parameter);
                AddBackStack(BackBehavior.SecondaryView, _ => TryLayerBack(), pageId);
                Navigating?.Invoke(this, args);
            }

            IsSecondaryViewShown = true;
        }

        /// <inheritdoc/>
        public void AddBackStack(BackBehavior id, Action<object> backBehavior, object parameter = null)
        {
            var args = new AppBackEventArgs(id, backBehavior, parameter);
            _backStack.Add(args);
            CanBack = _backStack.Count > 1;
        }

        /// <inheritdoc/>
        public void RemoveBackStack(BackBehavior id)
        {
            if (_backStack.Any(p => p.Id == id))
            {
                _backStack.Remove(_backStack.Last(p => p.Id == id));
            }

            CanBack = _backStack.Count > 1;
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
            }
            else if (last.Id == BackBehavior.SecondaryView)
            {
                NavigateToSecondaryView((PageIds)last.Parameter, null);
            }
            else if (last.Id == BackBehavior.OpenPlayer)
            {
                NavigateToPlayView(last.Parameter);
            }
        }
    }
}
