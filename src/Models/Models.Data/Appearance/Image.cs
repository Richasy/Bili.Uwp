// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.Models.Data.Appearance
{
    /// <summary>
    /// 图片，涉及视频封面，用户头像等.
    /// </summary>
    /// <remarks>
    /// 通常情况下，我们通过 Url 来显示图片，但对于哔哩哔哩服务来说，可以通过添加后缀的形式获取指定分辨率的图片。
    /// 根据一定的规则，我们可以使用哔哩哔哩的图片裁切服务直接获取低分辨率的图片，以提高图片加载速度，降低内存占用，
    /// 避免本地解码再限制分辨率造成的图片发糊的情况.
    /// </remarks>
    /// <example>
    /// https://i1.hdslb.com/bfs/archive/image.jpg@300w_300h_1c 就表示获取宽高均为 300px 的图片.
    /// 一般情况下，我们主要限制分辨率，限制图片质量的收益不大.
    /// </example>
    public sealed class Image
    {
        private readonly string _sourceUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="uri">图片地址.</param>
        public Image(string uri) => _sourceUri = Uri = uri;

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        /// <param name="uri">网址.</param>
        /// <param name="width">图片宽度（像素）.</param>
        /// <param name="height">图片高度（像素）.</param>
        /// <param name="resolutionResolver">
        /// 分辨率解析器，用于将宽高与链接组合，生成新的地址.
        /// 第一个参数为宽度 (width)，第二个参数为高度 (height)，输出为链接后缀.
        /// </param>
        public Image(
            string uri,
            double width,
            double height,
            Func<double, double, string> resolutionResolver)
        {
            _sourceUri = uri;
            Width = width;
            Height = height;
            Uri = uri + resolutionResolver(width, height);
        }

        /// <summary>
        /// 预设的宽度.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 预设的高度.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 地址.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 获取图片原始链接.
        /// </summary>
        /// <returns>图片地址. 如果没有修改过图片链接则返回 <see cref="Uri"/>.</returns>
        public Uri GetSourceUri()
            => new (_sourceUri ?? Uri);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Image image && _sourceUri == image._sourceUri;

        /// <inheritdoc/>
        public override int GetHashCode() => _sourceUri.GetHashCode();
    }
}
