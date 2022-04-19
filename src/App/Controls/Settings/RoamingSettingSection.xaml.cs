// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Locator.Uwp;
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

        private void OnAddressBoxSubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var text = sender.Text;
            if (string.IsNullOrEmpty(text?.Trim()))
            {
                sender.Text = ViewModel.RoamingAddress;
            }
            else
            {
                if (Uri.IsWellFormedUriString(text.Trim(), UriKind.Absolute))
                {
                    ViewModel.RoamingAddress = text.TrimEnd('/');
                }
                else
                {
                    var tip = ServiceLocator.Instance.GetService<IResourceToolkit>().GetLocaleString(Models.Enums.LanguageNames.InvalidAddress);
                    AppViewModel.Instance.ShowTip(tip, Models.Enums.App.InfoType.Error);
                    sender.Text = ViewModel.RoamingAddress;
                }
            }
        }
    }
}
