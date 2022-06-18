// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Player;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控件.
    /// </summary>
    public sealed partial class BiliMediaTransportControls
    {
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
