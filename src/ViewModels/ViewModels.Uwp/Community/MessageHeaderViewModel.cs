// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 消息头部视图模型.
    /// </summary>
    public sealed partial class MessageHeaderViewModel : ViewModelBase, IMessageHeaderViewModel
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private int _count;

        [ObservableProperty]
        private bool _isShowBadge;

        /// <inheritdoc/>
        public MessageType Type { get; set; }

        /// <inheritdoc/>
        public void SetData(MessageType type, int count = 0)
        {
            Type = type;
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            Title = type switch
            {
                MessageType.Reply => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Reply),
                MessageType.At => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.AtMe),
                MessageType.Like => resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.LikeMe),
                _ => string.Empty,
            };
            Count = count;
            IsShowBadge = Count > 0;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is MessageHeaderViewModel model && Type == model.Type;

        /// <inheritdoc/>
        public override int GetHashCode() => Type.GetHashCode();

        partial void OnCountChanged(int value)
            => IsShowBadge = value > 0;
    }
}
