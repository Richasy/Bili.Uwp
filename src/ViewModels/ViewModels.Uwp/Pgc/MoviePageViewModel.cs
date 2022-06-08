// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 电影页面视图模型.
    /// </summary>
    public sealed class MoviePageViewModel : PgcPageViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoviePageViewModel"/> class.
        /// </summary>
        public MoviePageViewModel(
            IPgcProvider pgcProvider,
            IResourceToolkit resourceToolkit,
            CoreDispatcher dispatcher,
            NavigationViewModel navigationViewModel)
            : base(pgcProvider, resourceToolkit, dispatcher, navigationViewModel, PgcType.Movie)
        {
        }
    }
}
