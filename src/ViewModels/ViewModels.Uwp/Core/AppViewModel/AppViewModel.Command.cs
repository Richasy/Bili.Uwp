// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using CommandLine;
using Microsoft.QueryStringDotNET;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Core
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

            await Parser.Default.ParseArguments<CommandLineViewModel>(ParseArguments(arguments))
                .MapResult(
                    async (CommandLineViewModel vm) =>
                        {
                            PlaySnapshot record = null;
                            if (!string.IsNullOrEmpty(vm.VideoId))
                            {
                                record = new PlaySnapshot(vm.VideoId, default, VideoType.Video);
                            }
                            else if (!string.IsNullOrEmpty(vm.SeasonId))
                            {
                                var seasonResult = int.TryParse(vm.SeasonId.Replace("ss", string.Empty), out var seasonId);

                                if (seasonResult)
                                {
                                    record = new PlaySnapshot("0", seasonId.ToString(), VideoType.Pgc);
                                    if (!string.IsNullOrEmpty(vm.EpisodeId))
                                    {
                                        if (int.TryParse(vm.EpisodeId.Replace("ep", string.Empty), out var episodeId))
                                        {
                                            record.VideoId = episodeId.ToString();
                                        }
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(vm.EpisodeId))
                            {
                                if (int.TryParse(vm.EpisodeId.Replace("ep", string.Empty), out var episodeId))
                                {
                                    record = new PlaySnapshot(episodeId.ToString(), default, VideoType.Pgc);
                                }
                            }
                            else if (!string.IsNullOrEmpty(vm.LiveId))
                            {
                                record = new PlaySnapshot(vm.LiveId, default, VideoType.Live);
                            }
                            else if (!string.IsNullOrEmpty(vm.SearchWord))
                            {
                                var keyword = vm.SearchWord.Replace("\"", string.Empty);
                                await Task.Delay(500);
                                _navigationViewModel.NavigateToSecondaryView(PageIds.Search, keyword);
                            }
                            else if (!string.IsNullOrEmpty(vm.PageId))
                            {
                                var result = Enum.TryParse(vm.PageId, true, out PageIds pageId);

                                if (result && pageId != PageIds.None)
                                {
                                    _navigationViewModel.Navigate(pageId);
                                }
                            }

                            if (record != null)
                            {
                                if (!string.IsNullOrEmpty(vm.PlayerMode))
                                {
                                    if (vm.PlayerMode.Equals("mini", StringComparison.OrdinalIgnoreCase))
                                    {
                                        record.DisplayMode = PlayerDisplayMode.CompactOverlay;
                                    }
                                    else if (vm.PlayerMode.Equals("screen", StringComparison.OrdinalIgnoreCase))
                                    {
                                        record.DisplayMode = PlayerDisplayMode.FullScreen;
                                    }
                                    else if (vm.PlayerMode.Equals("window", StringComparison.OrdinalIgnoreCase))
                                    {
                                        record.DisplayMode = PlayerDisplayMode.FullWindow;
                                    }
                                }

                                await GetDispatcher().RunAsync(CoreDispatcherPriority.High, () =>
                                {
                                    _navigationViewModel.NavigateToPlayView(record);
                                });
                            }
                        },
                    _ => Task.FromResult(-1));

            string[] ParseArguments(string commandLine)
            {
                var parmChars = commandLine.ToCharArray();
                var inQuote = false;
                for (var index = 0; index < parmChars.Length; index++)
                {
                    if (parmChars[index] == '"')
                    {
                        inQuote = !inQuote;
                    }

                    if (!inQuote && parmChars[index] == ' ')
                    {
                        parmChars[index] = '\n';
                    }
                }

                return new string(parmChars).Split('\n');
            }
        }

        /// <summary>
        /// 从参数初始化协议调用.
        /// </summary>
        /// <param name="link">协议调用链接.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeProtocolFromQueryAsync(Uri link)
        {
            PlaySnapshot record = null;
            var queryList = QueryString.Parse(link.Query.TrimStart('?'));
            if (link.Host.Equals(AppConstants.Protocol.PlayHost, StringComparison.OrdinalIgnoreCase))
            {
                var hasVideoId = queryList.TryGetValue(AppConstants.Protocol.VideoParam, out var videoId);
                var hasSeasonId = queryList.TryGetValue(AppConstants.Protocol.SeasonParam, out var seasonId);
                var hasEpisodeId = queryList.TryGetValue(AppConstants.Protocol.EpisodeParam, out var episodeId);
                var hasLiveId = queryList.TryGetValue(AppConstants.Protocol.LiveParam, out var liveId);

                if (hasVideoId)
                {
                    record = new PlaySnapshot(videoId, default, VideoType.Video);
                    var hasPgcSign = queryList.TryGetValue(AppConstants.Protocol.IsPgcParam, out var isPgc);
                    if (hasPgcSign && Convert.ToBoolean(isPgc))
                    {
                        record.VideoType = VideoType.Pgc;
                        record.NeedBiliPlus = true;
                    }
                }
                else if (hasSeasonId)
                {
                    var seasonResult = int.TryParse(seasonId.Replace("ss", string.Empty), out var seasonIdNum);
                    if (seasonResult)
                    {
                        record = new PlaySnapshot(default, seasonIdNum.ToString(), VideoType.Pgc);
                        if (hasEpisodeId)
                        {
                            if (int.TryParse(episodeId.Replace("ep", string.Empty), out var episodeIdNum))
                            {
                                record.VideoId = episodeIdNum.ToString();
                            }
                        }
                    }
                }
                else if (hasEpisodeId)
                {
                    if (int.TryParse(episodeId.Replace("ep", string.Empty), out var episodeIdNum))
                    {
                        record = new PlaySnapshot(episodeIdNum.ToString(), default, VideoType.Pgc);
                    }
                }
                else if (hasLiveId)
                {
                    record = new PlaySnapshot(liveId, default, VideoType.Live);
                }
            }
            else if (link.Host.Equals(AppConstants.Protocol.FindHost, StringComparison.OrdinalIgnoreCase))
            {
                var hasKeyword = queryList.TryGetValue(AppConstants.Protocol.KeywordParam, out var keyword);
                if (hasKeyword)
                {
                    await Task.Delay(500);
                    _navigationViewModel.NavigateToSecondaryView(PageIds.Search, keyword);
                }
            }
            else if (link.Host.Equals(AppConstants.Protocol.NavigateHost, StringComparison.OrdinalIgnoreCase))
            {
                var hasId = queryList.TryGetValue(AppConstants.Protocol.IdParam, out var id);
                var result = Enum.TryParse(id, true, out PageIds pageId);

                if (result && pageId != PageIds.None)
                {
                    _navigationViewModel.Navigate(pageId);
                }
            }

            if (record != null)
            {
                var hasMode = queryList.TryGetValue(AppConstants.Protocol.ModeParam, out var modeId);
                if (hasMode)
                {
                    if (modeId.Equals("mini", StringComparison.OrdinalIgnoreCase))
                    {
                        record.DisplayMode = PlayerDisplayMode.CompactOverlay;
                    }
                    else if (modeId.Equals("screen", StringComparison.OrdinalIgnoreCase))
                    {
                        record.DisplayMode = PlayerDisplayMode.FullScreen;
                    }
                    else if (modeId.Equals("window", StringComparison.OrdinalIgnoreCase))
                    {
                        record.DisplayMode = PlayerDisplayMode.FullWindow;
                    }
                }

                await GetDispatcher().RunAsync(CoreDispatcherPriority.High, () =>
                {
                    _navigationViewModel.NavigateToPlayView(record);
                });
            }
        }

        private CoreDispatcher GetDispatcher()
            => Splat.Locator.Current.GetService<CoreDispatcher>();
    }
}
