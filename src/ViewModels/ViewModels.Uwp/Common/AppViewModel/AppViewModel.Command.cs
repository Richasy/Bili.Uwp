// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using CommandLine;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 应用视图模型.
    /// </summary>
    public partial class AppViewModel
    {
        /// <summary>
        /// 从命令行参数初始化命令.
        /// </summary>
        /// <param name="arguments">命令行参数.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeCommandFromArgumentsAsync(string arguments)
        {
            if (string.IsNullOrEmpty(arguments) || !arguments.Contains(" "))
            {
                return;
            }

            await Parser.Default.ParseArguments<CommandLineViewModel>(arguments.Split(' '))
                .MapResult(
                    async (CommandLineViewModel vm) =>
                        {
                            var needPlay = false;
                            if (!string.IsNullOrEmpty(vm.VideoId))
                            {
                                needPlay = true;
                                var record = new CurrentPlayingRecord(vm.VideoId, 0, VideoType.Video);
                                OpenPlayer(record);
                            }
                            else if (!string.IsNullOrEmpty(vm.SeasonId))
                            {
                                needPlay = true;
                                var seasonResult = int.TryParse(vm.SeasonId.Replace("ss", string.Empty), out var seasonId);

                                if (seasonResult)
                                {
                                    var record = new CurrentPlayingRecord("0", seasonId, VideoType.Pgc);
                                    if (!string.IsNullOrEmpty(vm.EpisodeId))
                                    {
                                        if (int.TryParse(vm.EpisodeId.Replace("ep", string.Empty), out var episodeId))
                                        {
                                            record.VideoId = episodeId.ToString();
                                        }
                                    }

                                    OpenPlayer(record);
                                }
                            }
                            else if (!string.IsNullOrEmpty(vm.LiveId))
                            {
                                needPlay = true;
                                await AccountViewModel.Instance.TrySignInAsync(true);
                                var record = new CurrentPlayingRecord(vm.LiveId, 0, VideoType.Live);
                                OpenPlayer(record);
                            }
                            else if (!string.IsNullOrEmpty(vm.SearchWord))
                            {
                                SearchModuleViewModel.Instance.InputWords = vm.SearchWord;
                                SetOverlayContentId(PageIds.Search);
                            }
                            else if (!string.IsNullOrEmpty(vm.PageId))
                            {
                                var result = Enum.TryParse(vm.PageId, true, out PageIds pageId);

                                if (result && pageId != PageIds.None)
                                {
                                    var id = pageId.GetHashCode();
                                    if (id < 100)
                                    {
                                        // 主页面.
                                        SetMainContentId(pageId);
                                    }
                                    else if (id < 200)
                                    {
                                        // 需要登录后才能查看的页面.
                                        if (await AccountViewModel.Instance.TrySignInAsync(true))
                                        {
                                            SetOverlayContentId(pageId);
                                        }
                                    }
                                }
                            }

                            if (needPlay)
                            {
                                if (vm.IsMiniPlay)
                                {
                                    PlayerViewModel.Instance.PlayerDisplayMode = PlayerDisplayMode.CompactOverlay;
                                }
                                else if (vm.IsFullScreenPlay)
                                {
                                    PlayerViewModel.Instance.PlayerDisplayMode = PlayerDisplayMode.FullScreen;
                                }
                                else if (vm.IsFullWindowPlay)
                                {
                                    PlayerViewModel.Instance.PlayerDisplayMode = PlayerDisplayMode.FullWindow;
                                }
                            }
                        },
                    _ => Task.FromResult(-1));
        }
    }
}
