// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Enums;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// PGC 内容索引页面视图模型的接口定义.
    /// </summary>
    public interface IIndexPageViewModel : IInformationFlowViewModel<ISeasonItemViewModel>
    {
        /// <summary>
        /// 筛选条件集合.
        /// </summary>
        ObservableCollection<IIndexFilterViewModel> Filters { get; }

        /// <summary>
        /// 页面类型.
        /// </summary>
        string PageType { get; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 设置类型.
        /// </summary>
        /// <param name="type">PGC 类型.</param>
        void SetType(PgcType type);
    }
}
