// Copyright (c) GodLeaveMe. All rights reserved.

namespace Richasy.Bili.Models.Enums
{
    /// <summary>
    /// 登录二维码的状态.
    /// </summary>
    public enum QRCodeStatus
    {
        /// <summary>
        /// 二维码已过期.
        /// </summary>
        Expiried,

        /// <summary>
        /// 需要用户确认.
        /// </summary>
        NotConfirm,

        /// <summary>
        /// 已通过验证.
        /// </summary>
        Success,

        /// <summary>
        /// 授权过程中出现错误.
        /// </summary>
        Failed,
    }
}
