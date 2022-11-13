// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 哔哩漫游设置.
    /// </summary>
    public sealed partial class RoamingSettingSection : SettingSectionControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoamingSettingSection"/> class.
        /// </summary>
        public RoamingSettingSection()
            : base() => InitializeComponent();

        private void OnVideoAddressBoxSubmitted(Microsoft.UI.Xaml.Controls.AutoSuggestBox sender, Microsoft.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var text = sender.Text;
            if (string.IsNullOrEmpty(text?.Trim()))
            {
                ViewModel.RoamingVideoAddress = string.Empty;
            }
            else
            {
                if (Uri.IsWellFormedUriString(text.Trim(), UriKind.Absolute))
                {
                    ViewModel.RoamingVideoAddress = text.TrimEnd('/');
                    ShowTip(LanguageNames.SetAddressSuccess, true);
                }
                else
                {
                    ShowTip(LanguageNames.InvalidAddress, false);
                    sender.Text = ViewModel.RoamingVideoAddress;
                }
            }
        }

        private void OnViewAddressBoxSubmitted(Microsoft.UI.Xaml.Controls.AutoSuggestBox sender, Microsoft.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var text = sender.Text;
            if (string.IsNullOrEmpty(text?.Trim()))
            {
                ViewModel.RoamingViewAddress = string.Empty;
            }
            else
            {
                if (Uri.IsWellFormedUriString(text.Trim(), UriKind.Absolute))
                {
                    ViewModel.RoamingViewAddress = text.TrimEnd('/');
                    ShowTip(LanguageNames.SetAddressSuccess, true);
                }
                else
                {
                    ShowTip(LanguageNames.InvalidAddress, false);
                    sender.Text = ViewModel.RoamingViewAddress;
                }
            }
        }

        private void OnSearchAddressBoxSubmitted(Microsoft.UI.Xaml.Controls.AutoSuggestBox sender, Microsoft.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var text = sender.Text;
            if (string.IsNullOrEmpty(text?.Trim()))
            {
                ViewModel.RoamingSearchAddress = string.Empty;
            }
            else
            {
                if (Uri.IsWellFormedUriString(text.Trim(), UriKind.Absolute))
                {
                    ViewModel.RoamingSearchAddress = text.TrimEnd('/');
                    ShowTip(LanguageNames.SetAddressSuccess, true);
                }
                else
                {
                    ShowTip(LanguageNames.InvalidAddress, false);
                    sender.Text = ViewModel.RoamingSearchAddress;
                }
            }
        }

        private void ShowTip(LanguageNames text, bool isSuccess)
        {
            var type = isSuccess
                ? Models.Enums.App.InfoType.Success
                : Models.Enums.App.InfoType.Error;
            var tip = Locator.Instance.GetService<IResourceToolkit>().GetLocaleString(text);
            Locator.Instance.GetService<ICallerViewModel>().ShowTip(tip, type);
        }
    }
}
