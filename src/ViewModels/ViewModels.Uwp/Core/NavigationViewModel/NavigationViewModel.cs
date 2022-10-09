// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bili.Models.App.Args;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 处理导航的视图模型.
    /// </summary>
    public sealed partial class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewModel"/> class.
        /// </summary>
        public NavigationViewModel(IRecordViewModel recordViewModel)
        {
            _recordViewModel = recordViewModel;
            _backStack = new List<AppBackEventArgs>();

            IsMainViewShown = true;
            BackCommand = new RelayCommand(Back, () => CanBack);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void AddBackStack(BackBehavior id, Action<object> backBehavior, object parameter = null)
        {
            var args = new AppBackEventArgs(id, backBehavior, parameter);
            _backStack.RemoveAll(p => p.Equals(args));
            _backStack.Add(args);
            CheckBackStatus();
        }

        /// <inheritdoc/>
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
                _recordViewModel.DeleteLastPlayItemCommand.Execute(null);
            }
            else if (last.Id == BackBehavior.SecondaryView)
            {
                NavigateToSecondaryView((PageIds)last.Parameter, null);
                _recordViewModel.DeleteLastPlayItemCommand.Execute(null);
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

        partial void OnIsMainViewShownChanged(bool value)
        {
            if (value)
            {
                IsPlayViewShown = IsSecondaryViewShown = false;
            }
        }

        partial void OnIsSecondaryViewShownChanged(bool value)
        {
            if (value)
            {
                IsPlayViewShown = IsMainViewShown = false;
            }
        }

        partial void OnIsPlayViewShownChanged(bool value)
        {
            if (value)
            {
                IsMainViewShown = IsSecondaryViewShown = false;
            }
        }
    }
}
