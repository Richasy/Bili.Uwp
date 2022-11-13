// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Common
{
    /// <summary>
    /// 播放速率条目视图模型.
    /// </summary>
    public sealed partial class PlaybackRateItemViewModel : SelectableViewModelBase<double>, IPlaybackRateItemViewModel
    {
        private Action<double> _action;

        [ObservableProperty]
        private IRelayCommand _activeCommand;

        /// <inheritdoc/>
        public void InjectAction(Action<double> action)
        {
            _action = action;
            ActiveCommand = new RelayCommand(() => { _action.Invoke(Data); });
        }
    }
}
