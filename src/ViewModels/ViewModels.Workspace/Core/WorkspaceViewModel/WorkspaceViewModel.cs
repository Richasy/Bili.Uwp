// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using Bili.Models.Enums.Workspace;
using Bili.ViewModels.Interfaces.Core;
using Models.Workspace;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 工坊视图模型.
    /// </summary>
    public sealed partial class WorkspaceViewModel : ViewModelBase, IWorkspaceViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceViewModel"/> class.
        /// </summary>
        public WorkspaceViewModel()
        {
            Items = new ObservableCollection<NavigateItem>();
            InitializeItems();
            CurrentItem = Items.First();
        }

        private void InitializeItems()
        {
            var homeItem = new NavigateItem(NavigateTarget.Home, "首页", FluentSymbol.Home);
            var hotItem = new NavigateItem(NavigateTarget.Hot, "热门视频", FluentSymbol.Rocket);
            var dynamicItem = new NavigateItem(NavigateTarget.Dynamic, "动态", FluentSymbol.DesignIdeas);
            var liveItem = new NavigateItem(NavigateTarget.Live, "直播", FluentSymbol.Video);
            var historyItem = new NavigateItem(NavigateTarget.History, "观看历史", FluentSymbol.History);
            Items.Add(homeItem);
            Items.Add(hotItem);
            Items.Add(dynamicItem);
            Items.Add(liveItem);
            Items.Add(historyItem);
        }
    }
}
