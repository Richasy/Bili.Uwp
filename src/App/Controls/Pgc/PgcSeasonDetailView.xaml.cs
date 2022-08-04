// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Pgc;
using Splat;

namespace Bili.App.Controls
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
            ViewModel = Locator.Current.GetService<IPgcPlayerPageViewModel>();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public IPgcPlayerPageViewModel ViewModel { get; }
    }
}
