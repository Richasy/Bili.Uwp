// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.ViewModels.Uwp.Core;
using Windows.UI.Xaml;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控件.
    /// </summary>
    public sealed partial class BiliMediaTransportControls : ReactiveControl<MediaPlayerViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiliMediaTransportControls"/> class.
        /// </summary>
        public BiliMediaTransportControls()
        {
            DefaultStyleKey = typeof(BiliMediaTransportControls);
            Loaded += OnLoaded;
        }

        internal override void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is MediaPlayerViewModel oldVM)
            {
                oldVM.PropertyChanged -= OnViewModelPropertyChanged;
            }

            var vm = e.NewValue as MediaPlayerViewModel;
            vm.PropertyChanged -= OnViewModelPropertyChanged;
            vm.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ChangeVisualStateFromStatus();
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Status))
            {
                ChangeVisualStateFromStatus();
            }
        }

        private void ChangeVisualStateFromStatus()
        {
            if (ViewModel.Status == Models.Enums.PlayerStatus.Playing)
            {
                VisualStateManager.GoToState(this, "PlayingState", false);
            }
            else if (ViewModel.Status == Models.Enums.PlayerStatus.Pause
                || ViewModel.Status == Models.Enums.PlayerStatus.End)
            {
                VisualStateManager.GoToState(this, "PauseState", false);
            }
        }
    }
}
