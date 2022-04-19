// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;

namespace Richasy.Bili.App.Controls
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
            => InitializeComponent();

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

        private void ShowTip(LanguageNames text, bool isSuccess)
        {
            var type = isSuccess
                ? Models.Enums.App.InfoType.Success
                : Models.Enums.App.InfoType.Error;
            var tip = ServiceLocator.Instance.GetService<IResourceToolkit>().GetLocaleString(text);
            AppViewModel.Instance.ShowTip(tip, type);
        }
    }
}
