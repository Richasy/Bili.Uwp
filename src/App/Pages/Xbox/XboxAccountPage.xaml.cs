// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Account;

namespace Bili.App.Pages.Xbox
{
    /// <summary>
    /// Xbox 账户页面.
    /// </summary>
    public sealed partial class XboxAccountPage : XboxAccountPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XboxAccountPage"/> class.
        /// </summary>
        public XboxAccountPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }

    /// <summary>
    /// <see cref="XboxAccountPage"/> 的基类.
    /// </summary>
    public class XboxAccountPageBase : AppPage<XboxAccountPageViewModel>
    {
    }
}
