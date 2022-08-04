// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Controls.Dialogs;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 历史记录页面.
    /// </summary>
    public sealed partial class HistoryPage : HistoryPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryPage"/> class.
        /// </summary>
        public HistoryPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private async void OnClearButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var isClear = false;
            if (ViewModel.Items.Count > 0)
            {
                // Show dialog.
                var msg = Locator.Current.GetService<IResourceToolkit>().GetLocaleString(LanguageNames.ClearHistoryWarning);
                var dialog = new ConfirmDialog(msg);
                var result = await dialog.ShowAsync().AsTask();
                if (result == ContentDialogResult.Primary)
                {
                    isClear = true;
                }
            }

            if (isClear)
            {
                ViewModel.ClearCommand.Execute().Subscribe();
            }
        }
    }

    /// <summary>
    /// <see cref="HistoryPage"/> 的基类.
    /// </summary>
    public class HistoryPageBase : AppPage<IHistoryPageViewModel>
    {
    }
}
