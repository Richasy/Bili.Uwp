// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Pgc;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 电影页面.
    /// </summary>
    public sealed partial class MoviePage : MoviePageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoviePage"/> class.
        /// </summary>
        public MoviePage() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="MoviePage"/> 的基类.
    /// </summary>
    public class MoviePageBase : AppPage<MoviePageViewModel>
    {
    }
}
