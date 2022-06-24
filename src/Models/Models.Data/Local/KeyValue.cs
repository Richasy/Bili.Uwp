// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Local
{
    /// <summary>
    /// 键值对.
    /// </summary>
    /// <typeparam name="T">值类型.</typeparam>
    public class KeyValue<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValue{T}"/> class.
        /// </summary>
        /// <param name="key">键名.</param>
        /// <param name="value">值.</param>
        public KeyValue(string key, T value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// 键名.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值.
        /// </summary>
        public T Value { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is KeyValue<T> value && Key == value.Key;

        /// <inheritdoc/>
        public override int GetHashCode() => Key.GetHashCode();
    }
}
