// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Bili.Models.Data.Player;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控件.
    /// </summary>
    public sealed partial class BiliMediaTransportControls
    {
        private static void OnTransportVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as BiliMediaTransportControls;
            if (e.NewValue is Visibility vis)
            {
                if (vis == Visibility.Visible)
                {
                    VisualStateManager.GoToState(instance, "ControlPanelFadeInState", false);
                }
                else
                {
                    VisualStateManager.GoToState(instance, "ControlPanelFadeOutState", false);
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ChangeVisualStateFromStatus();
            ChangeVisualStateFromDisplayMode();
            _playPauseButton.Focus(FocusState.Programmatic);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Status))
            {
                ChangeVisualStateFromStatus();
            }
            else if (e.PropertyName == nameof(ViewModel.DisplayMode))
            {
                ChangeVisualStateFromDisplayMode();
            }
            else if (e.PropertyName == nameof(ViewModel.IsShowMediaTransport))
            {
                _playPauseButton.Focus(FocusState.Programmatic);
            }
        }

        private void OnVolumeSliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ViewModel.ChangeVolumeCommand.Execute(e.NewValue).Subscribe();
            }
        }

        private void OnFormatListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_formatListView.SelectedItem is FormatInformation info
                && ViewModel.CurrentFormat != info)
            {
                ViewModel.ChangeFormatCommand.Execute(info).Subscribe();
            }
        }

        private void OnNormalProgressContainerPointerEntered(object sender, PointerRoutedEventArgs e)
            => ViewModel.IsShowInteractionProgress = true;

        private void OnInteractionProgressContainerPointerExited(object sender, PointerRoutedEventArgs e)
            => ViewModel.IsShowInteractionProgress = false;

        private void OnInteractionProgressSliderValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (!ViewModel.IsShowInteractionProgress)
            {
                return;
            }

            ViewModel.ChangeProgressCommand.Execute(e.NewValue).Subscribe();
        }
    }
}
