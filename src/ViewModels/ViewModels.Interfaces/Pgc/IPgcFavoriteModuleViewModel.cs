// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// PGC收藏夹视图模型基类的接口定义.
    /// </summary>
    public interface IPgcFavoriteModuleViewModel : IInformationFlowViewModel<ISeasonItemViewModel>
    {
        /// <summary>
        /// 状态集合.
        /// </summary>
        ObservableCollection<int> StatusCollection { get; }

        /// <summary>
        /// 选中状态命令.
        /// </summary>
        ReactiveCommand<int, Unit> SetStatusCommand { get; }

        /// <summary>
        /// 状态.
        /// </summary>
        int Status { get; }

        /// <summary>
        /// 是否显示空白.
        /// </summary>
        bool IsEmpty { get; }
    }
}
