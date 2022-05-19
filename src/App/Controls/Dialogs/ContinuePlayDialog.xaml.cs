// Copyright (c) Richasy. All rights reserved.

using Bili.Locator.Uwp;
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
        private object _playVM = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuePlayDialog"/> class.
        /// </summary>
        public ContinuePlayDialog()
        {
            InitializeComponent();
            _navigationViewModel = Splat.Locator.Current.GetService<NavigationViewModel>();
            Loaded += OnLoadedAsync;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            var tool = ServiceLocator.Instance.GetService<ISettingsToolkit>();
            var title = tool.ReadLocalSetting(Models.Enums.SettingNames.ContinuePlayTitle, string.Empty);
            _playVM = await PlayerViewModel.Instance.GetInitViewModelFromLocalAsync();
            if (string.IsNullOrEmpty(title) || _playVM == null)
            {
                tool.WriteLocalSetting(Models.Enums.SettingNames.CanContinuePlay, false);
                tool.DeleteLocalSetting(Models.Enums.SettingNames.ContinuePlayTitle);
                Hide();
            }

            VideoTitle.Text = title;
        }

        private void OnContentDialogPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _navigationViewModel.NavigateToPlayView(_playVM);
        }

        private async void OnContentDialogCloseButtonClickAsync(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await PlayerViewModel.Instance.ClearInitViewModelAsync();
        }
    }
}
