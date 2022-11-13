// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Pgc;
using Windows.UI.Xaml.Navigation;

namespace Bili.Uwp.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 时间线页面.
    /// </summary>
    public sealed partial class TimelinePage : TimelinePageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimelinePage"/> class.
        /// </summary>
        public TimelinePage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PgcType type)
            {
                ViewModel.SetType(type);
            }
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();
    }

    /// <summary>
    /// <see cref="TimelinePage"/> 的基类.
    /// </summary>
    public class TimelinePageBase : AppPage<ITimelinePageViewModel>
    {
    }
}
