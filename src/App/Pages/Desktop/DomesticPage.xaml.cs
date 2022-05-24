// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Pgc;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 国创页面.
    /// </summary>
    public sealed partial class DomesticPage : DomesticPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomesticPage"/> class.
        /// </summary>
        public DomesticPage() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="DomesticPage"/> 的基类.
    /// </summary>
    public class DomesticPageBase : AppPage<DomesticPageViewModel>
    {
    }
}
