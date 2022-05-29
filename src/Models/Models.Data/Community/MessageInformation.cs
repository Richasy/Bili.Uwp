// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.Data.Appearance;
using Bili.Models.Enums.App;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 消息信息.
    /// </summary>
    public sealed class MessageInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageInformation"/> class.
        /// </summary>
        /// <param name="id">该消息标识符.</param>
        /// <param name="type">消息类型.</param>
        /// <param name="avatar">用户头像.</param>
        /// <param name="userName">用户名.</param>
        /// <param name="isMultipleUsers">是否为多用户.</param>
        /// <param name="publishTime">发布时间.</param>
        /// <param name="subtitle">副标题.</param>
        /// <param name="message">消息内容.</param>
        /// <param name="sourceContent">源内容.</param>
        /// <param name="sourceId">源内容标识符.</param>
        /// <param name="properties">键值对属性.</param>
        public MessageInformation(
            string id,
            MessageType type,
            Image avatar,
            string userName,
            bool isMultipleUsers,
            DateTime publishTime,
            string subtitle,
            string message,
            string sourceContent,
            string sourceId,
            Dictionary<string, string> properties = default)
        {
            Type = type;
            Avatar = avatar;
            UserName = userName;
            IsMultipleUsers = isMultipleUsers;
            PublishTime = publishTime;
            Subtitle = subtitle;
            Message = message;
            SourceContent = sourceContent;
            SourceId = sourceId;
            Id = id;
            Properties = properties;
        }

        /// <summary>
        /// 消息类型.
        /// </summary>
        public MessageType Type { get; }

        /// <summary>
        /// 用户头像.
        /// </summary>
        /// <remarks>
        /// 如果该条目有多个用户，则只取第一位用户的头像.
        /// </remarks>
        public Image Avatar { get; }

        /// <summary>
        /// 用户名.
        /// </summary>
        /// <remarks>
        /// 如果该条目有多个用户，则只取第一位用户的名称.
        /// </remarks>
        public string UserName { get; }

        /// <summary>
        /// 该条目是否为包含多个用户的混合信息.
        /// </summary>
        public bool IsMultipleUsers { get; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        public DateTime PublishTime { get; }

        /// <summary>
        /// 副标题.
        /// </summary>
        public string Subtitle { get; }

        /// <summary>
        /// 消息内容.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 源内容，比如是通过哪个动态或者视频做的评论.
        /// </summary>
        public string SourceContent { get; }

        /// <summary>
        /// 源内容的标识符.
        /// </summary>
        public string SourceId { get; }

        /// <summary>
        /// 该消息的标识符.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 附加的键值对属性.
        /// </summary>
        public Dictionary<string, string> Properties { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is MessageInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}
