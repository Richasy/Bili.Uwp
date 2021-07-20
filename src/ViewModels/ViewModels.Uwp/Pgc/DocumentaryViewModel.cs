// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 纪录片视图模型.
    /// </summary>
    public class DocumentaryViewModel : FeedPgcViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentaryViewModel"/> class.
        /// </summary>
        public DocumentaryViewModel()
            : base(PgcType.Documentary)
        {
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static DocumentaryViewModel Instance { get; } = new Lazy<DocumentaryViewModel>(() => new DocumentaryViewModel()).Value;
    }
}
