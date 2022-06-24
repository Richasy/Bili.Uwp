// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Pgc;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 电视剧页面.
    /// </summary>
    public sealed partial class TvPage : TvPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TvPage"/> class.
        /// </summary>
        public TvPage()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// <see cref="TvPage"/> 的基类.
    /// </summary>
    public class TvPageBase : AppPage<TvPageViewModel>
    {
    }
}
