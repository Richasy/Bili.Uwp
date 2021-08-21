// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的账户部分.
    /// </summary>
    public partial class BiliController
    {
        private MyInfo _myInfo;

        /// <summary>
        /// 已登录用户的账户数据.
        /// </summary>
        public MyInfo MyInfo
        {
            get => _myInfo;
            set
            {
                _myInfo = value;
                AccountChanged?.Invoke(this, _myInfo);
            }
        }

        /// <summary>
        /// 获取我的资料.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestMyProfileAsync()
        {
            if (await _authorizeProvider.IsTokenValidAsync() && IsNetworkAvailable)
            {
                try
                {
                    var profile = await _accountProvider.GetMyInformationAsync();
                    this.MyInfo = profile;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 请求用户空间数据.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task<UserSpaceInformation> RequestUserSpaceInformationAsync(int userId)
        {
            ThrowWhenNetworkUnavaliable();
            var data = await _accountProvider.GetUserSpaceInformationAsync(userId);
            if (data.VideoSet != null)
            {
                var args = new UserSpaceVideoIterationEventArgs(data.VideoSet, userId);
                UserSpaceVideoIteration?.Invoke(this, args);
            }

            return data.User;
        }

        /// <summary>
        /// 请求用户空间视频集.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="offsetId">偏移Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestUserSpaceVideoSetAsync(int userId, string offsetId)
        {
            ThrowWhenNetworkUnavaliable();
            var data = await _accountProvider.GetUserSpaceVideoSetAsync(userId, offsetId);
            var args = new UserSpaceVideoIterationEventArgs(data, userId);
            UserSpaceVideoIteration?.Invoke(this, args);
        }

        /// <summary>
        /// 修改用户关系(关注/取消关注).
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="isFollow">是否关注.</param>
        /// <returns>结果.</returns>
        public async Task<bool> ModifyUserRelationAsync(int userId, bool isFollow)
        {
            ThrowWhenNetworkUnavaliable();
            var result = await _accountProvider.ModifyUserRelationAsync(userId, isFollow);
            return result;
        }
    }
}
