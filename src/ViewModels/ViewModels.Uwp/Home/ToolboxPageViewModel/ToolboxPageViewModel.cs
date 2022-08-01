// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Home;
using Bili.ViewModels.Interfaces.Toolbox;
using Splat;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 工具箱页面视图模型.
    /// </summary>
    public sealed partial class ToolboxPageViewModel : ViewModelBase, IToolboxPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolboxPageViewModel"/> class.
        /// </summary>
        public ToolboxPageViewModel()
        {
            ToolCollection = new ObservableCollection<IToolboxItemViewModel>
            {
                GetItemViewModel(ToolboxItemType.AvBvConverter),
                GetItemViewModel(ToolboxItemType.CoverDownloader),
            };
        }

        private IToolboxItemViewModel GetItemViewModel(ToolboxItemType type)
        {
            var vm = Locator.Current.GetService<IToolboxItemViewModel>();
            vm.SetType(type);
            return vm;
        }
    }
}
