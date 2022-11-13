// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Base
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

        [ObservableProperty]
        private int _status;

        [ObservableProperty]
        private bool _isEmpty;

        /// <summary>
        /// 状态集合.
        /// </summary>
        public ObservableCollection<int> StatusCollection { get; }

        /// <summary>
        /// 选中状态命令.
        /// </summary>
        public IRelayCommand<int> SetStatusCommand { get; }
    }
}
