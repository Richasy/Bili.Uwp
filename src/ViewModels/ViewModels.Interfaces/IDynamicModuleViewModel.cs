// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Community;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 动态模块视图模型的接口定义.
    /// </summary>
    public interface IDynamicModuleViewModel : IInformationFlowViewModel<IDynamicItemViewModel>
    {
        /// <summary>
        /// 动态是否为空.
        /// </summary>
        bool IsEmpty { get; }
    }
}
