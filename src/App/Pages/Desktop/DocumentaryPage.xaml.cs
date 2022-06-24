// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Pgc;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 纪录片页面.
    /// </summary>
    public sealed partial class DocumentaryPage : DocumentaryPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentaryPage"/> class.
        /// </summary>
        public DocumentaryPage() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="DocumentaryPage"/> 的基类.
    /// </summary>
    public class DocumentaryPageBase : AppPage<DocumentaryPageViewModel>
    {
    }
}
