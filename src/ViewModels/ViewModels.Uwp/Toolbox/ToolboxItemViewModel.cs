// Copyright (c) Richasy. All rights reserved.

using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
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
        /// <param name="type">条目类型.</param>
        public ToolboxItemViewModel(ToolboxItemType type)
        {
            ServiceLocator.Instance.LoadService(out _resourceToolkit);
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
    }
}
