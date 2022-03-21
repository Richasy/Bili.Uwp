// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 播放器说明组件，包括发布者，标题和说明文本等.
    /// </summary>
    public sealed partial class PlayerDescriptor : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDescriptor"/> class.
        /// </summary>
        public PlayerDescriptor()
        {
            InitializeComponent();
        }

        private async void OnUserTappedAsync(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext as UserViewModel;
            await UserView.Instance.ShowAsync(dataContext);
        }

        private async void OnFollowButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            btn.IsEnabled = false;
            await ViewModel.Publisher.ToggleFollowStateAsync();
            btn.IsEnabled = true;
        }
    }
}
