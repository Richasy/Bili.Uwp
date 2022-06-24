// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Splat;

namespace Bili.App.Controls
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

        private void OnVideoAddressBoxSubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var text = sender.Text;
            if (string.IsNullOrEmpty(text?.Trim()))
            {
                sender.Text = ViewModel.RoamingVideoAddress;
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

        private void OnViewAddressBoxSubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var text = sender.Text;
            if (string.IsNullOrEmpty(text?.Trim()))
            {
                sender.Text = ViewModel.RoamingViewAddress;
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

        private void OnSearchAddressBoxSubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var text = sender.Text;
            if (string.IsNullOrEmpty(text?.Trim()))
            {
                sender.Text = ViewModel.RoamingSearchAddress;
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
            var tip = Locator.Current.GetService<IResourceToolkit>().GetLocaleString(text);
            Splat.Locator.Current.GetService<AppViewModel>().ShowTip(tip, type);
        }
    }
}
