// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频格式视图模型.
    /// </summary>
    public class VideoFormatViewModel : SelectableViewModelBase<VideoFormat>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFormatViewModel"/> class.
        /// </summary>
        /// <param name="format">视频格式.</param>
        /// <param name="isSelected">是否选中.</param>
        public VideoFormatViewModel(VideoFormat format, bool isSelected)
            : base(format, isSelected)
        {
        }
    }
}
