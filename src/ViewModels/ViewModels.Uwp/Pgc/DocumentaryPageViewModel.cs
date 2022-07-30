// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Uwp.Base;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 纪录片页面视图模型.
    /// </summary>
    public sealed class DocumentaryPageViewModel : PgcPageViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentaryPageViewModel"/> class.
        /// </summary>
        public DocumentaryPageViewModel(
             IPgcProvider pgcProvider,
             IResourceToolkit resourceToolkit,
             CoreDispatcher dispatcher,
             INavigationViewModel navigationViewModel)
             : base(pgcProvider, resourceToolkit, dispatcher, navigationViewModel, PgcType.Documentary)
        {
        }
    }
}
