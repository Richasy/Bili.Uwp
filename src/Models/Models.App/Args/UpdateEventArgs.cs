// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Other;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// 版本升级事件参数.
    /// </summary>
    public class UpdateEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateEventArgs"/> class.
        /// </summary>
        /// <param name="response">响应结果.</param>
        public UpdateEventArgs(GithubReleaseResponse response)
        {
            Version = response.TagName.Replace("v", string.Empty)
                .Replace(".pre-release", string.Empty);
            ReleaseTitle = response.Name;
            ReleaseDescription = response.Description;
            DownloadUrl = new Uri(response.Url);
            PublishTime = response.PublishTime.ToLocalTime();
            IsPreRelease = response.IsPreRelease;
        }

        /// <summary>
        /// 版本号.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 发布标题.
        /// </summary>
        public string ReleaseTitle { get; set; }

        /// <summary>
        /// 发布说明.
        /// </summary>
        public string ReleaseDescription { get; set; }

        /// <summary>
        /// 下载地址.
        /// </summary>
        public Uri DownloadUrl { get; set; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        public DateTime PublishTime { get; set; }

        /// <summary>
        /// 是否为预览版.
        /// </summary>
        public bool IsPreRelease { get; set; }
    }
}
