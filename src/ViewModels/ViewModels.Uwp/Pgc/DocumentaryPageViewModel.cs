﻿// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Uwp.Base;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 纪录片页面视图模型.
    /// </summary>
    public sealed class DocumentaryPageViewModel : PgcPageViewModelBase, IDocumentaryPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentaryPageViewModel"/> class.
        /// </summary>
        public DocumentaryPageViewModel(
             IPgcProvider pgcProvider,
             IResourceToolkit resourceToolkit,
             INavigationViewModel navigationViewModel)
             : base(pgcProvider, resourceToolkit, navigationViewModel, PgcType.Documentary)
        {
        }
    }
}
