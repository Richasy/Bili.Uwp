// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive.Linq;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 消息头部视图模型.
    /// </summary>
    public sealed class MessageHeaderViewModel : ViewModelBase, IMessageHeaderViewModel
    {
        /// <inheritdoc/>
        public MessageType Type { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [ObservableProperty]
        public string Title { get; set; }

        /// <summary>
        /// 消息数.
        /// </summary>
        [ObservableProperty]
        public int Count { get; set; }

        /// <summary>
        /// 是否显示徽章文本.
        /// </summary>
        [ObservableProperty]
        public bool IsShowBadge { get; set; }

        /// <inheritdoc/>
        public void SetData(MessageType type, int count = 0)
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
                .Subscribe(x => IsShowBadge = x > 0);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is MessageHeaderViewModel model && Type == model.Type;

        /// <inheritdoc/>
        public override int GetHashCode() => Type.GetHashCode();
    }
}
