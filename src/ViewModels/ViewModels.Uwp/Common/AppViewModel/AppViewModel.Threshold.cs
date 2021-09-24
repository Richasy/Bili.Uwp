// Copyright (c) GodLeaveMe. All rights reserved.

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 应用视图模型.
    /// </summary>
    public partial class AppViewModel
    {
        private const string WideWindowWidthKey = "WideWindowThresholdWidth";
        private const string MediumWindowWidthKey = "MediumWindowThresholdWidth";
        private const string NarrowWindowWidthKey = "NarrowWindowThresholdWidth";
        private const string MinimumWindowWidthKey = "MinimumWindowThresholdWidth";

        private const string WidePageWidthKey = "WidePageThresholdWidth";
        private const string MediumPageWidthKey = "MediumPageThresholdWidth";
        private const string NarrowPageWidthKey = "NarrowPageThresholdWidth";
        private const string MinimumPageWidthKey = "MinimumPageThresholdWidth";

        /// <summary>
        /// 宽窗口阈值宽度.
        /// </summary>
        public double WideWindowThresholdWidth => _resourceToolkit.GetResource<double>(WideWindowWidthKey);

        /// <summary>
        /// 中窗口阈值宽度.
        /// </summary>
        public double MediumWindowThresholdWidth => _resourceToolkit.GetResource<double>(MediumWindowWidthKey);

        /// <summary>
        /// 窄窗口阈值宽度.
        /// </summary>
        public double NarrowWindowThresholdWidth => _resourceToolkit.GetResource<double>(NarrowWindowWidthKey);

        /// <summary>
        /// 最小窗口阈值宽度.
        /// </summary>
        public double MinimumWindowThresholdWidth => _resourceToolkit.GetResource<double>(MinimumWindowWidthKey);

        /// <summary>
        /// 宽页面阈值宽度.
        /// </summary>
        public double WidePageThresholdWidth => _resourceToolkit.GetResource<double>(WidePageWidthKey);

        /// <summary>
        /// 中页面阈值宽度.
        /// </summary>
        public double MediumPageThresholdWidth => _resourceToolkit.GetResource<double>(MediumPageWidthKey);

        /// <summary>
        /// 窄页面阈值宽度.
        /// </summary>
        public double NarrowPageThresholdWidth => _resourceToolkit.GetResource<double>(NarrowPageWidthKey);

        /// <summary>
        /// 最小页面阈值宽度.
        /// </summary>
        public double MinimumPageThresholdWidth => _resourceToolkit.GetResource<double>(MinimumPageWidthKey);
    }
}
