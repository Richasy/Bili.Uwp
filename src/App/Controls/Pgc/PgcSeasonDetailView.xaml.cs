// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Pgc;
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
            ViewModel = Splat.Locator.Current.GetService<PgcPlayerPageViewModel>();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PgcPlayerPageViewModel ViewModel { get; }
    }
}
