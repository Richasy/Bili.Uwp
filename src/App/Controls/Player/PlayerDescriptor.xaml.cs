// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Controls.Dialogs;
using Bili.Locator.Uwp;
using Bili.Models.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
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

        private async void OnTagButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var tag = (sender as FrameworkElement).DataContext as VideoTag;
            var settingsToolkit = ServiceLocator.Instance.GetService<ISettingsToolkit>();
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            var isFirstClick = settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.IsFirstClickTag, true);

            if (isFirstClick)
            {
                var dialog = new ConfirmDialog(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FirstClickTagTip));
                var result = await dialog.ShowAsync();
                if (result != ContentDialogResult.Primary)
                {
                    return;
                }

                settingsToolkit.WriteLocalSetting(Models.Enums.SettingNames.IsFirstClickTag, false);
            }

            SearchModuleViewModel.Instance.InputWords = tag.Name;
            AppViewModel.Instance.SetOverlayContentId(Models.Enums.PageIds.Search);
        }
    }
}
