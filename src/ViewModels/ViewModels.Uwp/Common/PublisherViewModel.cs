// Copyright (c) Richasy. All rights reserved.

using System;
using Bilibili.App.Archive.V1;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 发布者视图模型.
    /// </summary>
    public class PublisherViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherViewModel"/> class.
        /// </summary>
        /// <param name="author">用户信息.</param>
        public PublisherViewModel(Author author)
            : this(author.Name, author.Face, Convert.ToInt32(author.Mid))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherViewModel"/> class.
        /// </summary>
        /// <param name="info">直播间用户基本信息.</param>
        public PublisherViewModel(LiveUserBasicInformation info)
            : this(info.UserName, info.Avatar)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherViewModel"/> class.
        /// </summary>
        /// <param name="info">发布者基本信息.</param>
        public PublisherViewModel(PublisherInfo info)
            : this(info.Publisher, info.PublisherAvatar, info.Mid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherViewModel"/> class.
        /// </summary>
        /// <param name="name">用户名.</param>
        /// <param name="avatar">头像.</param>
        /// <param name="id">用户Id.</param>
        public PublisherViewModel(string name, string avatar = "", int id = -1)
        {
            Id = id;
            Name = name ?? "--";
            if (!string.IsNullOrEmpty(avatar))
            {
                Avatar = avatar + "@60w_60h_1c_100q.jpg";
            }
        }

        /// <summary>
        /// 头像.
        /// </summary>
        [Reactive]
        public string Avatar { get; set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [Reactive]
        public string Name { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        public int Id { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PublisherViewModel model && Id == model.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();
    }
}
