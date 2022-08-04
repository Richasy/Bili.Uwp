// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.User;
using Bili.ViewModels.Interfaces.Account;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 关系用户页面视图模型基类的接口定义，是粉丝/关注页面的基础.
    /// </summary>
    public interface IRelationPageViewModel : IInformationFlowViewModel<IUserItemViewModel>
    {
        /// <summary>
        /// 对应的用户基础资料.
        /// </summary>
        UserProfile Profile { get; }

        /// <summary>
        /// 标题后缀.
        /// </summary>
        string TitleSuffix { get; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 设置用户资料.
        /// </summary>
        /// <param name="profile">用户资料.</param>
        void SetProfile(UserProfile profile);
    }
}
