﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Bili.Models.Data.Player;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控件.
    /// </summary>
    public sealed partial class BiliMediaTransportControls
    {
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ChangeVisualStateFromStatus();
            ChangeVisualStateFromDisplayMode();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
            => ViewModel.PropertyChanged -= OnViewModelPropertyChanged;

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
    }
}
