// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Interfaces.Article
{
    /// <summary>
    /// 文章收藏夹视图模型的接口定义.
    /// </summary>
    public interface IArticleFavoriteModuleViewModel : IInformationFlowViewModel<IArticleItemViewModel>
    {
        /// <summary>
        /// 是否显示空白.
        /// </summary>
        bool IsEmpty { get; }
    }
}
