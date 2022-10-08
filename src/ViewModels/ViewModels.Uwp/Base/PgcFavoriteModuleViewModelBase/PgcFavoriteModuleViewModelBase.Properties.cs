// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// PGC收藏夹视图模型.
    /// </summary>
    public partial class PgcFavoriteModuleViewModelBase
    {
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly FavoriteType _type;

        private bool _isEnd;

        /// <summary>
        /// 状态集合.
        /// </summary>
        public ObservableCollection<int> StatusCollection { get; }

        /// <summary>
        /// 选中状态命令.
        /// </summary>
        public IRelayCommand<int> SetStatusCommand { get; }

        /// <summary>
        /// 状态.
        /// </summary>
        [ObservableProperty]
        public int Status { get; set; }

        /// <summary>
        /// 是否显示空白.
        /// </summary>
        [ObservableProperty]
        public bool IsEmpty { get; set; }
    }
}
