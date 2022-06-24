// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive.Linq;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 消息头部视图模型.
    /// </summary>
    public sealed class MessageHeaderViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageHeaderViewModel"/> class.
        /// </summary>
        /// <param name="type">消息类型.</param>
        /// <param name="count">消息数.</param>
        internal MessageHeaderViewModel(MessageType type, int count = 0)
        {
            Type = type;
            var resourceToolkit = Splat.Locator.Current.GetService<IResourceToolkit>();
            Title = type switch
            {
                MessageType.Reply => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Reply),
                MessageType.At => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.AtMe),
                MessageType.Like => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.LikeMe),
                _ => string.Empty,
            };
            Count = count;
            IsShowBadge = Count == 0;

            this.WhenAnyValue(p => p.Count)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => IsShowBadge = x > 0);
        }

        /// <summary>
        /// 消息类型.
        /// </summary>
        public MessageType Type { get; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 消息数.
        /// </summary>
        [Reactive]
        public int Count { get; set; }

        /// <summary>
        /// 是否显示徽章文本.
        /// </summary>
        [Reactive]
        public bool IsShowBadge { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is MessageHeaderViewModel model && Type == model.Type;

        /// <inheritdoc/>
        public override int GetHashCode() => Type.GetHashCode();
    }
}
