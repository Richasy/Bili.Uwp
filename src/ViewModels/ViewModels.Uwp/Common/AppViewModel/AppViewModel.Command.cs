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

            await Parser.Default.ParseArguments<CommandLineViewModel>(ParseArguments(arguments))
                .MapResult(
                    async (CommandLineViewModel vm) =>
                        {
                            CurrentPlayingRecord record = null;
                            if (!string.IsNullOrEmpty(vm.VideoId))
                            {
                                record = new CurrentPlayingRecord(vm.VideoId, 0, VideoType.Video);
                            }
                            else if (!string.IsNullOrEmpty(vm.SeasonId))
                            {
                                var seasonResult = int.TryParse(vm.SeasonId.Replace("ss", string.Empty), out var seasonId);

                                if (seasonResult)
                                {
                                    record = new CurrentPlayingRecord("0", seasonId, VideoType.Pgc);
                                    if (!string.IsNullOrEmpty(vm.EpisodeId))
                                    {
                                        if (int.TryParse(vm.EpisodeId.Replace("ep", string.Empty), out var episodeId))
                                        {
                                            record.VideoId = episodeId.ToString();
                                        }
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(vm.LiveId))
                            {
                                record = new CurrentPlayingRecord(vm.LiveId, 0, VideoType.Live);
                            }
                            else if (!string.IsNullOrEmpty(vm.SearchWord))
                            {
                                SearchModuleViewModel.Instance.InputWords = vm.SearchWord.Replace("\"", string.Empty);
                                await Task.Delay(500);
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

                                OpenPlayer(record);
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
    }
}
