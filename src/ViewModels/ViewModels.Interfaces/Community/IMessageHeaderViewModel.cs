// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Enums.App;

namespace Bili.ViewModels.Interfaces.Community
{
    /// <summary>
    /// 消息头部视图模型的接口定义.
    /// </summary>
    public interface IMessageHeaderViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 消息类型.
        /// </summary>
        MessageType Type { get; }

        /// <summary>
        /// 标题.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 消息数.
        /// </summary>
        int Count { get; set; }

        /// <summary>
        /// 是否显示徽章文本.
        /// </summary>
        bool IsShowBadge { get; }

        /// <summary>
        /// 设置数据.
        /// </summary>
        /// <param name="type">消息类型.</param>
        /// <param name="count">消息数.</param>
        void SetData(MessageType type, int count = 0);
    }
}
