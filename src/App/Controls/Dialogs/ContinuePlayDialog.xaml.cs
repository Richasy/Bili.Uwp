// Copyright (c) Richasy. All rights reserved.

using Bili.Locator.Uwp;
using Bili.Models.App.Constants;
using Bili.Models.Data.Local;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Dialogs
{
    /// <summary>
    /// 继续播放对话框.
    /// </summary>
    public sealed partial class ContinuePlayDialog : ContentDialog
    {
        private readonly NavigationViewModel _navigationViewModel;
        private readonly AppViewModel _appViewModel;
        private PlaySnapshot _snapshot = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuePlayDialog"/> class.
        /// </summary>
        public ContinuePlayDialog()
        {
            InitializeComponent();
            _navigationViewModel = Splat.Locator.Current.GetService<NavigationViewModel>();
            _appViewModel = Splat.Locator.Current.GetService<AppViewModel>();
            Loaded += OnLoadedAsync;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            var settingsToolkit = Splat.Locator.Current.GetService<ISettingsToolkit>();
            var fileToolkit = Splat.Locator.Current.GetService<IFileToolkit>();
            var title = settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.ContinuePlayTitle, string.Empty);
            _snapshot = await _appViewModel.GetLastPlayItemAsync();
            if (string.IsNullOrEmpty(title) || _snapshot == null)
            {
                settingsToolkit.WriteLocalSetting(Models.Enums.SettingNames.CanContinuePlay, false);
                settingsToolkit.DeleteLocalSetting(Models.Enums.SettingNames.ContinuePlayTitle);
                Hide();
            }

            VideoTitle.Text = title;
        }

        private void OnContentDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            => _navigationViewModel.NavigateToPlayView(_snapshot);

        private async void OnContentDialogCloseButtonClickAsync(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            => await _appViewModel.DeleteLastPlayItemAsync();
    }
}
