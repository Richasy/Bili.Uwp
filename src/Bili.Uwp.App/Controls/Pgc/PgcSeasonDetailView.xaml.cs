// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.Uwp.App.Controls
{
    /// <summary>
    /// PGC详情视图.
    /// </summary>
    public sealed partial class PgcSeasonDetailView : CenterPopup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcSeasonDetailView"/> class.
        /// </summary>
        public PgcSeasonDetailView()
        {
            InitializeComponent();
            ViewModel = Locator.Instance.GetService<IPgcPlayerPageViewModel>();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public IPgcPlayerPageViewModel ViewModel { get; }
    }
}
