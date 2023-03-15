// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Pgc;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 剧集单集视图模型.
    /// </summary>
    public sealed partial class EpisodeItemViewModel : ViewModelBase, IEpisodeItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeItemViewModel"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字转换工具.</param>
        /// <param name="navigationViewModel">导航服务.</param>
        public EpisodeItemViewModel(
            INumberToolkit numberToolkit)
        {
            _numberToolkit = numberToolkit;

            PlayCommand = new AsyncRelayCommand(PlayAsync);
        }

        /// <inheritdoc/>
        public void InjectData(EpisodeInformation information)
        {
            Data = information;
            InitializeData();
        }

        private void InitializeData()
        {
            if (Data.CommunityInformation != null)
            {
                PlayCountText = _numberToolkit.GetCountText(Data.CommunityInformation.PlayCount);
                DanmakuCountText = _numberToolkit.GetCountText(Data.CommunityInformation.DanmakuCount);
            }

            if (Data.Identifier.Duration > 0)
            {
                DurationText = _numberToolkit.GetDurationText(TimeSpan.FromSeconds(Data.Identifier.Duration));
            }
        }

        private Task PlayAsync()
            => Utilities.PlayEpisodeWithIdAsync(Data.Identifier.Id);
    }
}
