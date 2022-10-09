// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.DI.Container;
using Bili.Models.App.Args;
using Bili.Toolkit.Interfaces;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Dialogs
{
    /// <summary>
    /// 更新提醒对话框.
    /// </summary>
    public sealed partial class UpgradeDialog : ContentDialog
    {
        private readonly UpdateEventArgs _eventArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeDialog"/> class.
        /// </summary>
        public UpgradeDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpgradeDialog"/> class.
        /// </summary>
        /// <param name="args">升级参数.</param>
        public UpgradeDialog(UpdateEventArgs args)
            : this()
        {
            _eventArgs = args;
            Initialize();
        }

        private void Initialize()
        {
            TitleBlock.Text = _eventArgs.ReleaseTitle;
            PreReleaseContainer.Visibility = _eventArgs.IsPreRelease ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
            PublishTimeBlock.Text = _eventArgs.PublishTime.ToString("yyyy/MM/dd HH:mm");
            MarkdownBlock.Text = _eventArgs.ReleaseDescription;
        }

        private async void ContentDialog_PrimaryButtonClickAsync(ContentDialog sender, ContentDialogButtonClickEventArgs args)
            => await Launcher.LaunchUriAsync(_eventArgs.DownloadUrl);

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var settingToolkit = Locator.Instance.GetService<ISettingsToolkit>();
            settingToolkit.WriteLocalSetting(Models.Enums.SettingNames.IgnoreVersion, _eventArgs.Version);
        }
    }
}
