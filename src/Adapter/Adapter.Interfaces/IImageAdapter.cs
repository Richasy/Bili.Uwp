// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Appearance;
using Bilibili.App.Dynamic.V2;
using Bilibili.Main.Community.Reply.V1;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 图片适配器接口，将视频封面、用户头像等转换为 <see cref="Image"/>.
    /// </summary>
    public interface IImageAdapter
    {
        /// <summary>
        /// 将图片地址转换为 <see cref="Image"/>.
        /// </summary>
        /// <param name="uri">图片地址.</param>
        /// <returns><see cref="Image"/>.</returns>
        Image ConvertToImage(string uri);

        /// <summary>
        /// 根据图片地址及宽高信息生成缩略图地址.
        /// </summary>
        /// <param name="uri">图片地址.</param>
        /// <param name="width">图片宽度.</param>
        /// <param name="height">图片高度.</param>
        /// <returns><see cref="Image"/>.</returns>
        Image ConvertToImage(string uri, double width, double height);

        /// <summary>
        /// 根据图片地址生成适用于视频卡片尺寸的缩略图地址.
        /// </summary>
        /// <param name="uri">图片地址.</param>
        /// <returns><see cref="Image"/>.</returns>
        Image ConvertToVideoCardCover(string uri);

        /// <summary>
        /// 根据图片地址生成适用于 PGC 的竖式封面.
        /// </summary>
        /// <param name="uri">图片地址.</param>
        /// <returns><see cref="Image"/>.</returns>
        Image ConvertToPgcCover(string uri);

        /// <summary>
        /// 根据图片地址生成适用于文章卡片尺寸的缩略图地址.
        /// </summary>
        /// <param name="uri">图片地址.</param>
        /// <returns><see cref="Image"/>.</returns>
        Image ConvertToArticleCardCover(string uri);

        /// <summary>
        /// 将动态的描述模块 <see cref="ModuleDesc"/> 转换为表情文本.
        /// </summary>
        /// <param name="description">动态的描述模块.</param>
        /// <returns><see cref="EmoteText"/>.</returns>
        EmoteText ConvertToEmoteText(ModuleDesc description);

        /// <summary>
        /// 将评论内容 <see cref="Content"/> 转换为表情文本.
        /// </summary>
        /// <param name="content">评论内容.</param>
        /// <returns><see cref="EmoteText"/>.</returns>
        EmoteText ConvertToEmoteText(Content content);
    }
}
