// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Controls.Dialogs;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 稍后再看页面.
    /// </summary>
    public sealed partial class ViewLaterPage : ViewLaterPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLaterPage"/> class.
        /// </summary>
        public ViewLaterPage()
        {
            InitializeComponent();
        }

        private async void OnClearButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var isClear = false;
            if (ViewModel.Items.Count > 0)
            {
                // Show dialog.
                var msg = Locator.Current
                    .GetService<IResourceToolkit>()
                    .GetLocaleString(LanguageNames.ClearViewLaterWarning);
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
    /// <see cref="ViewLaterPage"/> 的基类.
    /// </summary>
    public class ViewLaterPageBase : AppPage<ViewLaterPageViewModel>
    {
    }
}
