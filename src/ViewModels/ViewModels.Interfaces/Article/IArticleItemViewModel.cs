// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Models.Data.Article;
using Bili.ViewModels.Interfaces.Account;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Article
{
    /// <summary>
    /// 文章条目视图模型的接口定义.
    /// </summary>
    public interface IArticleItemViewModel : IInjectDataViewModel<ArticleInformation>, IInjectActionViewModel<IArticleItemViewModel>, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        IRelayCommand OpenInBroswerCommand { get; }

        /// <summary>
        /// 阅读命令.
        /// </summary>
        IRelayCommand ReadCommand { get; }

        /// <summary>
        /// 取消收藏命令.
        /// </summary>
        IRelayCommand UnfavoriteCommand { get; }

        /// <summary>
        /// 阅读次数的可读文本.
        /// </summary>
        string ViewCountText { get; }

        /// <summary>
        /// 点赞数的可读文本.
        /// </summary>
        string LikeCountText { get; }

        /// <summary>
        /// 评论数的可读文本.
        /// </summary>
        string CommentCountText { get; }

        /// <summary>
        /// 是否显示社区信息.
        /// </summary>
        bool IsShowCommunity { get; }

        /// <summary>
        /// 发布者.
        /// </summary>
        IUserItemViewModel Publisher { get; }

        /// <summary>
        /// 获取文章详情内容.
        /// </summary>
        /// <returns>文章HTML文本.</returns>
        Task<string> GetDetailAsync();
    }
}
