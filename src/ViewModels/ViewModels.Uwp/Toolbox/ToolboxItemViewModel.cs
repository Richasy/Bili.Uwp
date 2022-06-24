// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Toolbox
{
    /// <summary>
    /// 工具箱条目视图模型.
    /// </summary>
    public sealed class ToolboxItemViewModel : ViewModelBase
    {
        private readonly IResourceToolkit _resourceToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolboxItemViewModel"/> class.
        /// </summary>
        public ToolboxItemViewModel(IResourceToolkit resourceToolkit)
            => _resourceToolkit = resourceToolkit;

        /// <summary>
        /// 类型.
        /// </summary>
        [Reactive]
        public ToolboxItemType Type { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 描述.
        /// </summary>
        [Reactive]
        public string Description { get; set; }

        /// <summary>
        /// 设置工具类型.
        /// </summary>
        /// <param name="type">工具类型.</param>
        public void SetType(ToolboxItemType type)
        {
            Type = type;
            switch (type)
            {
                case ToolboxItemType.AvBvConverter:
                    Title = _resourceToolkit.GetLocaleString(LanguageNames.AvBvConverter);
                    Description = _resourceToolkit.GetLocaleString(LanguageNames.AvBvConverterDescription);
                    break;
                case ToolboxItemType.CoverDownloader:
                    Title = _resourceToolkit.GetLocaleString(LanguageNames.CoverDownloader);
                    Description = _resourceToolkit.GetLocaleString(LanguageNames.CoverDownloaderDescription);
                    break;
                default:
                    break;
            }
        }
    }
}
