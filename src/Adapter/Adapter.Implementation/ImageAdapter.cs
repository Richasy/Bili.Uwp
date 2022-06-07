// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bili.Adapter.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.Data.Appearance;
using Bilibili.App.Dynamic.V2;
using Bilibili.Main.Community.Reply.V1;

namespace Bili.Adapter
{
    /// <summary>
    /// 图片适配器，将视频封面、用户头像等转换为 <see cref="Image"/>.
    /// </summary>
    public class ImageAdapter : IImageAdapter
    {
        /// <inheritdoc/>
        public Image ConvertToImage(string uri)
            => new Image(uri);

        /// <inheritdoc/>
        public Image ConvertToImage(string uri, double width, double height)
            => new Image(uri, width, height, (w, h) => $"@{w}w_{h}h_1c_100q.jpg");

        /// <inheritdoc/>
        public Image ConvertToVideoCardCover(string uri)
            => ConvertToImage(uri, AppConstants.VideoCardCoverWidth, AppConstants.VideoCardCoverHeight);

        /// <inheritdoc/>
        public Image ConvertToPgcCover(string uri)
            => ConvertToImage(uri, AppConstants.PgcCoverWidth, AppConstants.PgcCoverHeight);

        /// <inheritdoc/>
        public Image ConvertToArticleCardCover(string uri)
            => ConvertToImage(uri, AppConstants.ArticleCardCoverWidth, AppConstants.ArticleCardCoverHeight);

        /// <inheritdoc/>
        public EmoteText ConvertToEmoteText(ModuleDesc description)
        {
            var text = description.Text;
            var descs = description.Desc;
            var emoteDict = new Dictionary<string, Image>();

            // 判断是否有表情存在.
            if (descs.Count > 0 && descs.Any(p => p.Type == DescType.Emoji))
            {
                var emotes = descs.Where(p => p.Type == DescType.Emoji);
                foreach (var item in emotes)
                {
                    if (!emoteDict.ContainsKey(item.Text))
                    {
                        emoteDict.Add(item.Text, ConvertToImage(item.Uri));
                    }
                }
            }
            else
            {
                emoteDict = null;
            }

            return new EmoteText(text, emoteDict);
        }

        /// <inheritdoc/>
        public EmoteText ConvertToEmoteText(Content content)
        {
            var text = content.Message;
            var emotes = content.Emote;
            var emoteDict = new Dictionary<string, Image>();

            // 判断是否有表情存在.
            if (emotes?.Count > 0)
            {
                foreach (var item in emotes)
                {
                    if (!emoteDict.ContainsKey(item.Key))
                    {
                        emoteDict.Add(item.Key, ConvertToImage(item.Value.Url));
                    }
                }
            }
            else
            {
                emoteDict = null;
            }

            return new EmoteText(text, emoteDict);
        }
    }
}
