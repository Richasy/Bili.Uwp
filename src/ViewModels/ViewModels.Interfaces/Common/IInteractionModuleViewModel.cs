// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.Data.Player;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Common
{
    /// <summary>
    /// 互动视频模块视图模型.
    /// </summary>
    public interface IInteractionModuleViewModel : IReactiveObject, IReloadViewModel
    {
        /// <summary>
        /// 无法获取到选项时发生.
        /// </summary>
        event EventHandler NoMoreChoices;

        /// <summary>
        /// 选择集合.
        /// </summary>
        ObservableCollection<InteractionInformation> Choices { get; }

        /// <summary>
        /// 设置初始数据.
        /// </summary>
        /// <param name="partId">视频分P Id.</param>
        /// <param name="choiceId">选项 Id.</param>
        /// <param name="graphVersion">互动版本.</param>
        void SetData(string partId, string choiceId, string graphVersion);
    }
}
