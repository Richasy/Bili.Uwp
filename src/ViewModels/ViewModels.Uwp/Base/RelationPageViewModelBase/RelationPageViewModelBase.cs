// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// 关系用户页面视图模型基类，是粉丝/关注页面的基础.
    /// </summary>
    public partial class RelationPageViewModelBase : InformationFlowViewModelBase<IUserItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelationPageViewModelBase"/> class.
        /// </summary>
        internal RelationPageViewModelBase(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            RelationType type,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;
            _relationType = type;

            TitleSuffix = type switch
            {
                RelationType.Follows => _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FollowsSuffix),
                _ => _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.FansSuffix)
            };
        }

        /// <summary>
        /// 设置用户资料.
        /// </summary>
        /// <param name="profile">用户资料.</param>
        public void SetProfile(UserProfile profile)
        {
            Profile = profile;
            TryClear(Items);
            BeforeReload();
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _isEnd = false;
            IsEmpty = false;
            _accountProvider.ResetRelationStatus(_relationType);
        }

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (_isEnd)
            {
                return;
            }

            var data = await _accountProvider.GetUserFansOrFollowsAsync(Profile.Id, _relationType);
            foreach (var item in data.Accounts)
            {
                var userVM = Locator.Instance.GetService<IUserItemViewModel>();
                userVM.SetInformation(item);
                Items.Add(userVM);
            }

            _isEnd = Items.Count == data.TotalCount || Items.Count > 200;
            IsEmpty = Items.Count == 0;
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
        {
            var prefix = _relationType == RelationType.Follows
                ? _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestFollowsFailed)
                : _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestFansFailed);
            return $"{prefix}\n{errorMsg}";
        }
    }
}
