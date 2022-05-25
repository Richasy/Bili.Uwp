// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 时间线页面.
    /// </summary>
    public sealed partial class TimeLinePage : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeLinePage"/> class.
        /// </summary>
        public TimeLinePage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PgcType vm)
            {
            }

            base.OnNavigatedTo(e);
        }
    }
}
