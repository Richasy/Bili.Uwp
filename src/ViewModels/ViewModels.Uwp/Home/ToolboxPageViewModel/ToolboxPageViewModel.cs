// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Enums;
using Bili.ViewModels.Uwp.Toolbox;
using Splat;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 工具箱页面视图模型.
    /// </summary>
    public sealed partial class ToolboxPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolboxPageViewModel"/> class.
        /// </summary>
        public ToolboxPageViewModel()
        {
            ToolCollection = new ObservableCollection<ToolboxItemViewModel>
            {
                GetItemViewModel(ToolboxItemType.AvBvConverter),
                GetItemViewModel(ToolboxItemType.CoverDownloader),
            };
        }

        private ToolboxItemViewModel GetItemViewModel(ToolboxItemType type)
        {
            var vm = Splat.Locator.Current.GetService<ToolboxItemViewModel>();
            vm.SetType(type);
            return vm;
        }
    }
}
