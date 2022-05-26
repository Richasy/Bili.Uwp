// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.Enums;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 工具箱页面视图模型.
    /// </summary>
    public sealed partial class ToolboxPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolboxPageViewModel"/> class.
        /// </summary>
        internal ToolboxPageViewModel()
        {
            ToolCollection = new ObservableCollection<ToolboxItemViewModel>
            {
                new ToolboxItemViewModel(ToolboxItemType.AvBvConverter),
                new ToolboxItemViewModel(ToolboxItemType.CoverDownloader),
            };
        }

        /// <summary>
        /// 获取视频Id类型.
        /// </summary>
        /// <param name="id">视频Id.</param>
        /// <param name="avId">解析后的 Aid.</param>
        /// <returns><c>av</c> 表示 AV Id, <c>bv</c> 表示 BV Id, 空表示不规范.</returns>
        public static VideoIdType GetVideoIdType(string id, out long avId)
        {
            avId = 0;
            if (id.StartsWith("bv", StringComparison.OrdinalIgnoreCase))
            {
                // 判定为 BV Id.
                return VideoIdType.Bv;
            }
            else
            {
                // 可能是 AV Id.
                id = id.Replace("av", string.Empty, StringComparison.OrdinalIgnoreCase);
                return long.TryParse(id, out avId) ? VideoIdType.Av : VideoIdType.Invalid;
            }
        }
    }
}
