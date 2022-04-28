// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.Models.App.Other
{
    /// <summary>
    /// 本地缓存基本类型.
    /// </summary>
    /// <typeparam name="T">存储的数据类型.</typeparam>
    public class LocalCache<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalCache{T}"/> class.
        /// </summary>
        /// <param name="expiryTime">过期时间.</param>
        /// <param name="data">要存储的数据.</param>
        public LocalCache(DateTimeOffset expiryTime, T data)
        {
            this.ExpiryTime = expiryTime;
            this.Data = data;
        }

        /// <summary>
        /// 过期时间.
        /// </summary>
        public DateTimeOffset ExpiryTime { get; set; }

        /// <summary>
        /// 缓存的数据.
        /// </summary>
        public T Data { get; set; }
    }
}
