// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.App.Other;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// 我的收藏页面视图模型.
    /// </summary>
    public interface IFavoritePageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 类型集合.
        /// </summary>
        ObservableCollection<FavoriteHeader> TypeCollection { get; }

        /// <summary>
        /// 选择收藏类型的命令.
        /// </summary>
        IRelayCommand<FavoriteHeader> SelectTypeCommand { get; }

        /// <summary>
        /// 当前选中项.
        /// </summary>
        FavoriteHeader CurrentType { get; }

        /// <summary>
        /// 是否显示视频收藏.
        /// </summary>
        bool IsVideoShown { get; }

        /// <summary>
        /// 是否显示动漫收藏.
        /// </summary>
        bool IsAnimeShown { get; }

        /// <summary>
        /// 是否显示影视收藏.
        /// </summary>
        bool IsCinemaShown { get; }

        /// <summary>
        /// 是否显示文章收藏.
        /// </summary>
        bool IsArticleShown { get; }
    }
}
