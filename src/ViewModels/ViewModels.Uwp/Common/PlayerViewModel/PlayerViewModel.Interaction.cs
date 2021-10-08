// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        /// <summary>
        /// 回退跳转.
        /// </summary>
        /// <param name="seconds">跳转秒数.</param>
        public void BackSkip(double seconds)
        {
            if (seconds <= 0 || PlayerStatus == PlayerStatus.NotLoad || PlayerStatus == PlayerStatus.Buffering)
            {
                return;
            }

            try
            {
                var currentPos = _currentVideoPlayer.PlaybackSession.Position;
                if (currentPos.TotalSeconds > seconds)
                {
                    currentPos -= TimeSpan.FromSeconds(seconds);
                }
                else
                {
                    currentPos = TimeSpan.Zero;
                }

                _currentVideoPlayer.PlaybackSession.Position = currentPos;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 前进跳转.
        /// </summary>
        /// <param name="seconds">跳转秒数.</param>
        public void ForwardSkip(double seconds)
        {
            if (seconds <= 0 || PlayerStatus == PlayerStatus.NotLoad || PlayerStatus == PlayerStatus.Buffering)
            {
                return;
            }

            try
            {
                var duration = _currentVideoPlayer.PlaybackSession.NaturalDuration;
                var currentPos = _currentVideoPlayer.PlaybackSession.Position;
                if ((duration - currentPos).TotalSeconds > seconds)
                {
                    currentPos += TimeSpan.FromSeconds(seconds);
                }
                else
                {
                    currentPos = duration;
                }

                _currentVideoPlayer.PlaybackSession.Position = currentPos;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 一键三连.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task TripleAsync()
        {
            var aid = GetAid();
            var result = await Controller.TripleAsync(aid);

            if (result != null)
            {
                IsLikeChecked = result.IsLike;
                IsCoinChecked = result.IsCoin;
                IsFavoriteChecked = result.IsFavorite;
                await InitVideoStatusAsync();
            }
        }

        /// <summary>
        /// 点赞视频.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LikeAsync()
        {
            var isLike = !IsLikeChecked;
            var aid = GetAid();
            var isSuccess = await Controller.LikeVideoAsync(aid, isLike);

            if (isSuccess)
            {
                IsLikeChecked = isLike;
                await InitVideoStatusAsync();
            }
        }

        /// <summary>
        /// 投币.
        /// </summary>
        /// <param name="number">投币个数.</param>
        /// <param name="isAlsoLike">是否同时点赞.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task CoinAsync(int number, bool isAlsoLike)
        {
            var aid = GetAid();
            var result = await Controller.CoinVideoAsync(aid, number, isAlsoLike);
            if (result != null)
            {
                IsCoinChecked = true;
                if (result.IsAlsoLike)
                {
                    IsLikeChecked = true;
                }

                await InitVideoStatusAsync();
            }
        }

        /// <summary>
        /// 收藏视频.
        /// </summary>
        /// <param name="selectedIds">选中的收藏夹Id.</param>
        /// <param name="deselectedIds">取消选中的收藏夹Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task FavoriteAsync()
        {
            var aid = GetAid();
            var selectedIds = FavoriteMetaCollection.Where(p => p.IsSelected).Select(p => p.Data.Id).ToList();
            var deselectedIds = FavoriteMetaCollection.Where(p => !p.IsSelected).Select(p => p.Data.Id).ToList();
            var result = await Controller.FavoriteVideoAsync(aid, selectedIds, deselectedIds);

            switch (result)
            {
                case Models.Enums.Bili.FavoriteResult.Success:
                case Models.Enums.Bili.FavoriteResult.InsufficientAccess:
                    IsFavoriteChecked = selectedIds.Count > 0;
                    await InitVideoStatusAsync();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 追番/取消追番.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ToggleFollowAsync()
        {
            if (!IsPgc)
            {
                return;
            }

            var seasonId = Convert.ToInt32(SeasonId);
            var isSuccess = await Controller.FollowPgcSeasonAsync(seasonId, !IsFollow);
            if (isSuccess)
            {
                IsFollow = !IsFollow;
            }
        }

        /// <summary>
        /// 改变选项.
        /// </summary>
        /// <param name="choice">选项.</param>
        public void ChangeChoice(InteractionChoice choice)
        {
            _interactionPartId = choice.PartId;
            _interactionNodeId = choice.Id;
        }

        /// <summary>
        /// 回到互动视频起点.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task BackToInteractionStartAsync()
        {
            _interactionPartId = _videoDetail.Pages.First().Page.Cid;
            _interactionNodeId = 0;
            await InitializeInteractionVideoAsync();
        }

        /// <summary>
        /// 初始化互动视频选项.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeInteractionVideoAsync()
        {
            if (_isInteractionChanging)
            {
                return;
            }

            _isInteractionChanging = true;
            try
            {
                var response = await Controller.GetInteractionEdgeAsync(Convert.ToInt32(_videoId), _videoDetail.Interaction.GraphVersion.ToString(), _interactionNodeId);
                _interactionDetail = response;
                ChoiceCollection.Clear();
                IsShowChoice = false;
                IsShowInteractionEnd = false;
                if (_interactionDetail?.Edges?.Questions?.Any() ?? false)
                {
                    var choices = _interactionDetail.Edges.Questions.First().Choices;
                    foreach (var choice in choices)
                    {
                        if (!string.IsNullOrEmpty(choice.Condition))
                        {
                            var variables = _interactionDetail.HiddenVariables.Where(p => choice.Condition.Contains(p.Id));
                            if (variables != null)
                            {
                                var minString = Regex.Match(choice.Condition, ">=([0-9]{1,}[.][0-9]*)").Value.Replace(">=", string.Empty);
                                var maxString = Regex.Match(choice.Condition, "<=([0-9]{1,}[.][0-9]*)").Value.Replace("<=", string.Empty);
                                var min = string.IsNullOrEmpty(minString) ? 0 : Convert.ToDouble(minString);
                                var max = string.IsNullOrEmpty(maxString) ? -1 : Convert.ToDouble(maxString);
                                var variable = variables.Where(p => choice.Condition.Contains(p.Id)).FirstOrDefault();
                                if (variable != null)
                                {
                                    if (variable.Value >= min && (max == -1 || variable.Value <= max))
                                    {
                                        ChoiceCollection.Add(choice);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ChoiceCollection.Add(choice);
                        }
                    }
                }

                await ChangeVideoPartAsync(_interactionPartId);
            }
            catch (Exception ex)
            {
                IsPlayInformationError = true;
                PlayInformationErrorText = _resourceToolkit.GetLocaleString(LanguageNames.FailedToLoadInteractionVideo);
                _logger.LogError(ex);
            }

            _isInteractionChanging = false;
        }

        private long GetAid()
        {
            return IsPgc ? CurrentPgcEpisode.Aid : _videoId;
        }
    }
}
