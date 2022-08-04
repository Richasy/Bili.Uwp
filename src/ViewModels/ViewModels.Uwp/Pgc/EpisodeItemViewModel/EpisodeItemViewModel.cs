// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Pgc
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
            INumberToolkit numberToolkit,
            INavigationViewModel navigationViewModel)
        {
            _numberToolkit = numberToolkit;
            _navigationViewModel = navigationViewModel;

            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync);
            PlayCommand = ReactiveCommand.Create(Play);
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

        private void Play()
            => _navigationViewModel.NavigateToPlayView(new Models.Data.Local.PlaySnapshot(Data.Identifier.Id, Data.SeasonId, Models.Enums.VideoType.Pgc)
            {
                Title = Data.Identifier.Title,
            });

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://www.bilibili.com/bangumi/play/ep{Data.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}
