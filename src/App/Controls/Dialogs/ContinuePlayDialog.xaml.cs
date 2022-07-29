// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Local;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
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
        private readonly IRecordViewModel _recordViewModel;
        private PlaySnapshot _snapshot = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuePlayDialog"/> class.
        /// </summary>
        public ContinuePlayDialog()
        {
            InitializeComponent();
            _navigationViewModel = Splat.Locator.Current.GetService<NavigationViewModel>();
            _recordViewModel = Locator.Current.GetService<IRecordViewModel>();
            Loaded += OnLoadedAsync;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            var settingsToolkit = Locator.Current.GetService<ISettingsToolkit>();
            _snapshot = await _recordViewModel.GetLastPlayItemAsync();
            if (_snapshot == null)
            {
                settingsToolkit.WriteLocalSetting(Models.Enums.SettingNames.CanContinuePlay, false);
                Hide();
            }

            VideoTitle.Text = _snapshot.Title ?? string.Empty;
        }

        private void OnContentDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            => _navigationViewModel.NavigateToPlayView(_snapshot);

        private void OnContentDialogCloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            => _recordViewModel.DeleteLastPlayItemCommand.Execute().Subscribe();
    }
}
