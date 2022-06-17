// Copyright (c) Richasy. All rights reserved.

using System;
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
    }
}
