// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Bili;
using Bili.Models.Enums.Community;
using Bilibili.App.Interfaces.V1;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib
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
            _relationOffsetCache = new Dictionary<RelationType, int>();
            _myFollowOffsetCache = new Dictionary<string, int>();
            ResetViewLaterStatus();
            ResetHistoryStatus();
        }

        /// <inheritdoc/>
        public int UserId { get; set; }

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
        public async Task<UserSpaceView> GetUserSpaceInformationAsync(string userId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Space, queryParameters, forceNoToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<UserSpaceResponse>>(response);
            var accInfo = _userAdapter.ConvertToAccountInformation(result.Data.User, AvatarSize.Size96);
            var videoSet = _videoAdapter.ConvertToVideoSet(result.Data.VideoSet);
            if (videoSet.Items.Count() > 0)
            {
                _spaceVideoOffset = videoSet.Items.Last().Identifier.Id;
            }

            return new UserSpaceView(accInfo, videoSet);
        }

        /// <inheritdoc/>
        public async Task<VideoSet> GetUserSpaceVideoSetAsync(string userId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId },
                { Query.Aid, _spaceVideoOffset.ToString() },
                { Query.Order, "pubdate" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.VideoCursor, queryParameters, forceNoToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<UserSpaceVideoSet>>(response);
            var data = _videoAdapter.ConvertToVideoSet(result.Data);
            if (data.Items.Count() > 0)
            {
                _spaceVideoOffset = data.Items.Last().Identifier.Id;
            }

            return data;
        }

        /// <inheritdoc/>
        public async Task<bool> ModifyUserRelationAsync(string userId, bool isFollow)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Fid, userId },
                { Query.ReSrc, "21" },
                { Query.ActionSlim, isFollow ? "1" : "2" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ModifyRelation, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);

            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<RelationView> GetUserFansOrFollowsAsync(string userId, RelationType type)
        {
            var hasCache = _relationOffsetCache.TryGetValue(type, out var page);
            if (!hasCache)
            {
                page = 1;
            }

            var queryParameters = new Dictionary<string, string>
            {
                { Query.VMid, userId },
                { Query.PageNumber, page.ToString() },
            };

            var uri = type == RelationType.Fans
                    ? Account.Fans
                    : Account.Follows;

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, uri, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var parsed = await _httpProvider.ParseAsync<ServerResponse<RelatedUserResponse>>(response);
            ResetRelationStatus(type);
            page++;
            _relationOffsetCache.Add(type, page);
            return _userAdapter.ConvertToRelationView(parsed.Data);
        }

        /// <inheritdoc/>
        public async Task<VideoSet> GetViewLaterListAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PageNumber, _viewLaterPageNumber.ToString() },
                { Query.PageSizeSlim, "40" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.ViewLaterList, queryParameters, Models.Enums.RequestClientType.IOS, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<ViewLaterResponse>>(response);
            _viewLaterPageNumber++;
            return _videoAdapter.ConvertToVideoSet(result.Data);
        }

        /// <inheritdoc/>
        public async Task<bool> ClearViewLaterAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ViewLaterClear, type: Models.Enums.RequestClientType.IOS, needToken: true);
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
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ViewLaterAdd, queryParameters, Models.Enums.RequestClientType.IOS, needToken: true);
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
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.ViewLaterDelete, queryParameters, Models.Enums.RequestClientType.IOS, needToken: true);
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
        public async Task<UserRelationStatus> GetRelationAsync(string targetUserId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Fid, targetUserId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.Relation, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<UserRelationResponse>>(response);
            return result.Data.Type switch
            {
                2 => UserRelationStatus.Following,
                6 => UserRelationStatus.Friends,
                _ => UserRelationStatus.Unfollow,
            };
        }

        /// <inheritdoc/>
        public async Task<VideoSet> SearchUserSpaceVideoAsync(string userId, string keyword)
        {
            var req = new SearchArchiveReq
            {
                Mid = System.Convert.ToInt64(userId),
                Keyword = keyword,
                Pn = _spaceSearchPageNumber,
                Ps = 20,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Account.SpaceVideoSearch, req, false);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, SearchArchiveReply.Parser);
            _spaceSearchPageNumber++;
            var videoSet = _videoAdapter.ConvertToVideoSet(data);
            foreach (var item in videoSet.Items)
            {
                item.Publisher = null;
            }

            return videoSet;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FollowGroup>> GetMyFollowingGroupsAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.MyFollowingTags, null, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<List<RelatedTag>>>(response);
            return result.Data.Select(p => _communityAdapter.ConvertToFollowGroup(p));
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AccountInformation>> GetMyFollowingGroupDetailAsync(string tagId)
        {
            if (UserId <= 0)
            {
                throw new System.InvalidOperationException("未登录");
            }

            if (!_myFollowOffsetCache.TryGetValue(tagId, out var page))
            {
                page = 1;
            }

            var queryParameters = new Dictionary<string, string>
            {
                { Query.TagId, tagId.ToString() },
                { Query.PageNumber, page.ToString() },
                { Query.MyId, UserId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.MyFollowingTagDetail, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<List<RelatedUser>>>(response);
            page++;
            _myFollowOffsetCache.Remove(tagId);
            _myFollowOffsetCache.Add(tagId, page);
            result.Data.ForEach(p => p.Attribute = 2);
            return result.Data.Select(p => _userAdapter.ConvertToAccountInformation(p));
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

        /// <inheritdoc/>
        public void ResetRelationStatus(RelationType type)
            => _relationOffsetCache.Remove(type);

        /// <inheritdoc/>
        public void ResetSpaceVideoStatus()
            => _spaceVideoOffset = string.Empty;

        /// <inheritdoc/>
        public void ResetSpaceSearchStatus()
            => _spaceSearchPageNumber = 1;

        /// <inheritdoc/>
        public void ResetMyFollowStatus(string groupId)
            => _myFollowOffsetCache.Remove(groupId);

        /// <inheritdoc/>
        public void ClearMyFollowStatus()
            => _myFollowOffsetCache.Clear();
    }
}
