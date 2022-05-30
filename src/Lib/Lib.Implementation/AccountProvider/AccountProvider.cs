// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Models.Enums.App;
using Bilibili.App.Interfaces.V1;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 提供已登录用户的数据操作.
    /// </summary>
    public partial class AccountProvider : IAccountProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        /// <param name="userAdapter">用户适配器.</param>
        /// <param name="communityAdapter">社区数据适配器.</param>
        /// <param name="videoAdapter">视频数据适配器.</param>
        public AccountProvider(
            IHttpProvider httpProvider,
            IUserAdapter userAdapter,
            ICommunityAdapter communityAdapter,
            IVideoAdapter videoAdapter)
        {
            _httpProvider = httpProvider;
            _userAdapter = userAdapter;
            _communityAdapter = communityAdapter;
            _videoAdapter = videoAdapter;
            _messageOffsetCache = new Dictionary<MessageType, MessageCursor>();
            ResetViewLaterStatus();
            ResetHistoryStatus();
        }

        /// <inheritdoc/>
        public int UserId { get; private set; }

        /// <inheritdoc/>
        public async Task<AccountInformation> GetMyInformationAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.MyInfo, type: Models.Enums.RequestClientType.IOS, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<MyInfo>>(response);
            UserId = result.Data.Mid;
            return _userAdapter.ConvertToAccountInformation(result.Data, Models.Enums.App.AvatarSize.Size48);
        }

        /// <inheritdoc/>
        public async Task<UserCommunityInformation> GetMyCommunityInformationAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Mine, type: Models.Enums.RequestClientType.IOS, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<Mine>>(response);
            UserId = result.Data.Mid;
            return _communityAdapter.ConvertToUserCommunityInformation(result.Data);
        }

        /// <inheritdoc/>
        public async Task<VideoHistoryView> GetMyHistorySetAsync(string tabSign = "archive")
        {
            var req = new CursorV2Req
            {
                Business = tabSign,
                Cursor = _historyCursor,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Account.HistoryCursor, req, true);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, CursorV2Reply.Parser);
            _historyCursor = data.Cursor;
            return _videoAdapter.ConvertToVideoHistoryView(data);
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveHistoryItemAsync(string itemId, string tabSign = "archive")
        {
            var req = new DeleteReq
            {
                HisInfo = new HisInfo
                {
                    Business = tabSign,
                    Kid = System.Convert.ToInt64(itemId),
                },
            };

            var request = await _httpProvider.GetRequestMessageAsync(Account.DeleteHistoryItem, req, true);
            _ = await _httpProvider.SendAsync(request);
            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> ClearHistoryAsync(string tabSign = "archive")
        {
            var req = new ClearReq() { Business = tabSign };
            var request = await _httpProvider.GetRequestMessageAsync(Account.ClearHistory, req, true);
            _ = await _httpProvider.SendAsync(request);
            return true;
        }

        /// <inheritdoc/>
        public async Task<UserSpaceResponse> GetUserSpaceInformationAsync(int userId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Space, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<UserSpaceResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<UserSpaceVideoSet> GetUserSpaceVideoSetAsync(int userId, string offsetId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId.ToString() },
                { Query.Aid, offsetId },
                { Query.Order, "pubdate" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.VideoCursor, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<UserSpaceVideoSet>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<bool> ModifyUserRelationAsync(int userId, bool isFollow)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Fid, userId.ToString() },
                { Query.ReSrc, "21" },
                { Query.ActionSlim, isFollow ? "1" : "2" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ModifyRelation, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);

            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<RelatedUserResponse> GetFansAsync(int userId, int page)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId.ToString() },
                { Query.Page, page.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Fans, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<RelatedUserResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<RelatedUserResponse> GetFollowsAsync(int userId, int page)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId.ToString() },
                { Query.PageNumber, page.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Follows, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<RelatedUserResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<ViewLaterView> GetViewLaterListAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PageNumber, _viewLaterPageNumber.ToString() },
                { Query.PageSizeSlim, "40" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.ViewLaterList, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<ViewLaterResponse>>(response);
            _viewLaterPageNumber++;
            return _videoAdapter.ConvertToViewLaterView(result.Data);
        }

        /// <inheritdoc/>
        public async Task<bool> ClearViewLaterAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ViewLaterClear, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> AddVideoToViewLaterAsync(string videoId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ViewLaterAdd, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveVideoFromViewLaterAsync(params string[] videoIds)
        {
            var ids = string.Join(",", videoIds);
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, ids },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ViewLaterDelete, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<FavoriteListResponse> GetFavoriteListAsync(int userId, int videoId = 0)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.UpId, userId.ToString() },
                { Query.Type, "2" },
                { Query.PartitionId, videoId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.FavoriteList, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<FavoriteListResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<VideoFavoriteGalleryResponse> GetFavoriteVideoGalleryAsync(int userId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.UpId, userId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.VideoFavoriteGallery, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<VideoFavoriteGalleryResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<VideoFavoriteListResponse> GetFavoriteVideoListAsync(int favoriteId, int pageNumber)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.MediaId, favoriteId.ToString() },
                { Query.PageSizeSlim, "20" },
                { Query.PageNumber, pageNumber.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.VideoFavoriteDelta, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<VideoFavoriteListResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public Task<PgcFavoriteListResponse> GetFavoriteAnimeListAsync(int pageNumber, int status)
            => GetPgcFavoriteListInternalAsync(Account.AnimeFavorite, pageNumber, status);

        /// <inheritdoc/>
        public Task<PgcFavoriteListResponse> GetFavoriteCinemaListAsync(int pageNumber, int status)
            => GetPgcFavoriteListInternalAsync(Account.CinemaFavorite, pageNumber, status);

        /// <inheritdoc/>
        public async Task<bool> UpdateFavoritePgcStatusAsync(int seasonId, int status)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.SeasonId, seasonId.ToString() },
                { Query.Status, status.ToString() },
                { Query.Device, "phone" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.UpdatePgcStatus, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.Message == "success";
        }

        /// <inheritdoc/>
        public async Task<ArticleFavoriteListResponse> GetFavortieArticleListAsync(int pageNumber)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSizeSlim, "20" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.ArticleFavorite, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<ArticleFavoriteListResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<FavoriteMediaList> GetFavoriteFolderListAsync(int userId, int pageNumber, bool isCreated)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.UpId, userId.ToString() },
                { Query.PageSizeSlim, "20" },
                { Query.PageNumber, pageNumber.ToString() },
            };

            var url = isCreated ? Account.CreatedVideoFavoriteFolderDelta : Account.CollectedVideoFavoriteFolderDelta;
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, url, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<FavoriteMediaList>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFavoriteFolderAsync(int favoriteId, bool isMe)
        {
            var queryParameters = new Dictionary<string, string>();
            string uri;
            if (isMe)
            {
                uri = Account.DeleteFavoriteFolder;
                queryParameters.Add(Query.MediaIds, favoriteId.ToString());
            }
            else
            {
                uri = Account.UnFavoriteFolder;
                queryParameters.Add(Query.MediaId, favoriteId.ToString());
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, uri, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFavoriteVideoAsync(int favoriteId, int videoId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.MediaId, favoriteId.ToString() },
                { Query.Resources, $"{videoId}:2" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.UnFavoriteVideo, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFavoritePgcAsync(int seasonId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.SeasonId, seasonId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.UnFavoritePgc, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFavoriteArticleAsync(int articleId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Id, articleId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.UnFavoriteArticle, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<UnreadInformation> GetUnreadMessageAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.MessageUnread, null, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<UnreadMessage>>(response);
            return _communityAdapter.ConvertToUnreadInformation(result.Data);
        }

        /// <inheritdoc/>
        public async Task<MessageView> GetMyMessagesAsync(MessageType type)
        {
            var id = 0L;
            var time = 0L;
            if (_messageOffsetCache.TryGetValue(type, out var cache))
            {
                id = cache.Id;
                time = cache.Time;
            }

            var data = await GetMessageInternalAsync(type, id, time);
            return data;
        }

        /// <inheritdoc/>
        public async Task<UserRelationResponse> GetRelationAsync(int targetUserId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Fid, targetUserId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Relation, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<UserRelationResponse>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<SearchArchiveReply> SearchUserSpaceVideoAsync(int userId, string keyword, int pageNumber, int pageSize = 20)
        {
            var req = new SearchArchiveReq
            {
                Mid = userId,
                Keyword = keyword,
                Pn = pageNumber,
                Ps = pageSize,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Account.SpaceVideoSearch, req, false);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, SearchArchiveReply.Parser);
            return data;
        }

        /// <inheritdoc/>
        public async Task<List<RelatedTag>> GetMyFollowingTagsAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.MyFollowingTags, null, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<List<RelatedTag>>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<List<RelatedUser>> GetMyFollowingTagDetailAsync(int userId, int tagId, int page)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.TagId, tagId.ToString() },
                { Query.PageNumber, page.ToString() },
                { Query.MyId, userId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.MyFollowingTagDetail, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<List<RelatedUser>>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public void ClearMessageStatus()
            => _messageOffsetCache.Clear();

        /// <inheritdoc/>
        public void ResetViewLaterStatus()
            => _viewLaterPageNumber = 0;

        /// <inheritdoc/>
        public void ResetHistoryStatus()
            => _historyCursor = new Cursor { Max = 0 };
    }
}
