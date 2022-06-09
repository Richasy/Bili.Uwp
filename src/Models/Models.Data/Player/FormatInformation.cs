// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 格式信息.
    /// </summary>
    public sealed class FormatInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormatInformation"/> class.
        /// </summary>
        /// <param name="quality">视频清晰度.</param>
        /// <param name="description">清晰度描述.</param>
        /// <param name="isLimited">是否需要登录或大会员才能观看.</param>
        public FormatInformation(int quality, string description, bool isLimited)
        {
            Quality = quality;
            Description = description;
            IsLimited = isLimited;
        }

        /// <summary>
        /// 视频清晰度.
        /// </summary>
        public int Quality { get; }

        /// <summary>
        /// 清晰度描述.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 是否需要登录或大会员才能观看.
        /// </summary>
        public bool IsLimited { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FormatInformation information && Quality == information.Quality;

        /// <inheritdoc/>
        public override int GetHashCode() => Quality.GetHashCode();
    }
}
