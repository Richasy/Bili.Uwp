// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 互动条目信息.
    /// </summary>
    public sealed class InteractionInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionInformation"/> class.
        /// </summary>
        public InteractionInformation(
            string id,
            string condition,
            string partId,
            string text,
            bool isValid)
        {
            Id = id;
            Condition = condition;
            PartId = partId;
            Text = text;
            IsValid = isValid;
        }

        /// <summary>
        /// 标识符.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 条件语句.
        /// </summary>
        public string Condition { get; }

        /// <summary>
        /// 对应的分P Id.
        /// </summary>
        public string PartId { get; }

        /// <summary>
        /// 选项文本.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 该选项是否有效.
        /// </summary>
        public bool IsValid { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is InteractionInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}
