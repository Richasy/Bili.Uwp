// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Live
{
    /// <summary>
    /// 直播弹幕信息.
    /// </summary>
    public sealed class LiveDanmakuInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveDanmakuInformation"/> class.
        /// </summary>
        /// <param name="text">文本.</param>
        /// <param name="textColor">文本颜色.</param>
        /// <param name="userName">用户名.</param>
        /// <param name="level">用户等级.</param>
        /// <param name="levelColor">用户等级颜色 (Hex).</param>
        /// <param name="isAdmin">是否为管理员.</param>
        public LiveDanmakuInformation(
            string text,
            string textColor,
            string userName,
            string level,
            string levelColor,
            bool isAdmin)
        {
            Text = text;
            TextColor = textColor;
            UserName = userName;
            UserLevel = level;
            UserLevelColor = levelColor;
            IsAdmin = isAdmin;
        }

        /// <summary>
        /// 文本.
        /// </summary>
        public string Text { get; }

        public string TextColor { get; }

        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// 用户等级.
        /// </summary>
        public string UserLevel { get; }

        /// <summary>
        /// 等级颜色.
        /// </summary>
        public string UserLevelColor { get; }

        /// <summary>
        /// 是否为管理员.
        /// </summary>
        public bool IsAdmin { get; }
    }
}
