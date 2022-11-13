// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Interfaces.Community;

namespace Bili.Uwp.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 消息页面.
    /// </summary>
    public sealed partial class MessagePage : MessagePageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePage"/> class.
        /// </summary>
        public MessagePage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnNavItemInvokedAsync(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            ContentScrollViewer.ChangeView(0, 0, 1, true);
            var data = args.InvokedItem as IMessageHeaderViewModel;

            if (data != ViewModel.CurrentType)
            {
                ViewModel.SelectTypeCommand.Execute(data);
            }
        }
    }

    /// <summary>
    /// <see cref="MessagePage"/> 的基类.
    /// </summary>
    public class MessagePageBase : AppPage<IMessagePageViewModel>
    {
    }
}
