// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.ViewModels.Interfaces;
using DynamicData;
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
            _history = new SourceList<AppNavigationEventArgs>();

            var canBackObserve = _history.CountChanged.Select(_ => _history.Count > 1);

            _canBack = canBackObserve.ToProperty(this, x => x.CanBack, scheduler: RxApp.MainThreadScheduler);

            BackCommand = ReactiveCommand.Create<object>(Back, canBackObserve, RxApp.MainThreadScheduler);

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
        public void NavigateToMainView(PageIds pageId, object parameter)
        {
            var args = new AppNavigationEventArgs(NavigationType.Main, pageId, parameter);
            var result = AddNewArgsToHistory(args);
            IsMainViewShown = true;

            if (result)
            {
                Navigating?.Invoke(this, args);
            }
        }

        /// <inheritdoc/>
        public void NavigateToPlayView(object parameter)
        {
            var args = new AppNavigationEventArgs(NavigationType.Player, PageIds.Player, parameter);
            var result = AddNewArgsToHistory(args);
            IsPlayViewShown = true;

            if (result)
            {
                Navigating?.Invoke(this, args);
            }
        }

        /// <inheritdoc/>
        public void NavigateToSecondaryView(PageIds pageId, object parameter)
        {
            var args = new AppNavigationEventArgs(NavigationType.Secondary, pageId, parameter);
            var result = AddNewArgsToHistory(args);
            IsSecondaryViewShown = true;

            if (result)
            {
                Navigating?.Invoke(this, args);
            }
        }

        private void Back(object parameter)
        {
            var last = _history.Items.Last();
            var previous = _history.Items.SkipLast(1).LastOrDefault();
            var backArgs = new AppBackEventArgs(previous?.Type ?? NavigationType.Main, previous?.PageId ?? default, parameter ?? previous.Parameter);
            if (previous != null)
            {
                Navigate(previous.PageId, previous.Parameter);
            }

            _history.Remove(last);
            Backing?.Invoke(this, backArgs);
        }

        private bool AddNewArgsToHistory(AppNavigationEventArgs args)
        {
            var last = _history.Items.LastOrDefault();
            if (last?.Equals(args) ?? false)
            {
                return false;
            }

            _history.RemoveMany(_history.Items.Where(p => p.Type == NavigationType.Main));
            _history.Add(args);
            return true;
        }
    }
}
