// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Data.Player;
using Bili.Models.Enums.App;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Common
{
    /// <summary>
    /// 字幕模块视图模型的接口定义.
    /// </summary>
    public interface ISubtitleModuleViewModel : INotifyPropertyChanged, IReloadViewModel
    {
        /// <summary>
        /// 重新定位命令.
        /// </summary>
        IRelayCommand<double> SeekCommand { get; }

        /// <summary>
        /// 更换字幕的命令.
        /// </summary>
        IRelayCommand<SubtitleMeta> ChangeMetaCommand { get; }

        /// <summary>
        /// 字幕元数据集合.
        /// </summary>
        ObservableCollection<SubtitleMeta> Metas { get; }

        /// <summary>
        /// 字幕转换类型集合.
        /// </summary>
        ObservableCollection<SubtitleConvertType> ConvertTypeCollection { get; }

        /// <summary>
        /// 当前字幕.
        /// </summary>
        string CurrentSubtitle { get; set; }

        /// <summary>
        /// 当前的元数据.
        /// </summary>
        SubtitleMeta CurrentMeta { get; set; }

        /// <summary>
        /// 字幕转换类型.
        /// </summary>
        SubtitleConvertType ConvertType { get; set; }

        /// <summary>
        /// 是否有字幕.
        /// </summary>
        bool HasSubtitles { get; set; }

        /// <summary>
        /// 是否需要显示字幕.
        /// </summary>
        bool CanShowSubtitle { get; set; }

        /// <summary>
        /// 设置初始数据.
        /// </summary>
        /// <param name="mainId">主 Id, 比如视频的 Aid.</param>
        /// <param name="partId">分部 Id, 比如视频的 Cid.</param>
        void SetData(string mainId, string partId);
    }
}
