// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp.Common;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 弹幕输入控件.
    /// </summary>
    public sealed partial class DanmakuBox : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuBox"/> class.
        /// </summary>
        public DanmakuBox()
        {
            this.InitializeComponent();
            ViewModel = DanmakuViewModel.Instance;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DanmakuViewModel ViewModel { get; private set; }

        private async void OnDanmakuInputBoxSubmittedAsync(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.QueryText))
            {
                sender.IsEnabled = false;
                var result = await ViewModel.SendDanmakuAsync(args.QueryText);
                sender.IsEnabled = true;

                if (result)
                {
                    sender.Text = string.Empty;
                }
            }
        }
    }
}
