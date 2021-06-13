// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.Models.Enums
{
    /// <summary>
    /// 授权结果状态.
    /// </summary>
    public enum AuthorizeResultStatus
    {
        /// <summary>
        /// 登录成功
        /// </summary>
        Success,

        /// <summary>
        /// 登录失败
        /// </summary>
        Fail,

        /// <summary>
        /// 登录错误
        /// </summary>
        Error,

        /// <summary>
        /// 登录需要验证码
        /// </summary>
        NeedCaptcha,

        /// <summary>
        /// 需要安全认证
        /// </summary>
        NeedValidate,

        /// <summary>
        /// 登录繁忙.
        /// </summary>
        Busy,

        /// <summary>
        /// 未指定的错误.
        /// </summary>
        Unspecific,
    }
}
