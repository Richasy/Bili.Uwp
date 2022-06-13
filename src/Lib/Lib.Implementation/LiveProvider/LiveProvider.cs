// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.BiliBili;
using Bili.Models.Data.Live;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib
{
    /// <summary>
    /// 提供直播相关的数据操作.
    /// </summary>
    public partial class LiveProvider : ILiveProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络请求处理工具.</param>
        /// <param name="accountProvider">账户工具.</param>
        /// <param name="liveAdapter">直播数据适配工具.</param>
        /// <param name="communityAdapter">社区数据适配工具.</param>
        public LiveProvider(
            IHttpProvider httpProvider,
            IAccountProvider accountProvider,
            ILiveAdapter liveAdapter,
            ICommunityAdapter communityAdapter)
        {
            _httpProvider = httpProvider;
            _accountProvider = accountProvider;
            _liveAdapter = liveAdapter;
            _communityAdapter = communityAdapter;

            _feedPageNumber = 1;
            _partitionPageNumber = 1;
        }

        /// <inheritdoc/>
        public event EventHandler<LiveMessageEventArgs> MessageReceived;

        /// <inheritdoc/>
        public async Task<LiveFeedView> GetLiveFeedsAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Page, _feedPageNumber.ToString() },
                { Query.RelationPage, _feedPageNumber.ToString() },
                { Query.Scale, "2" },
                { Query.LoginEvent, "1" },
                { Query.Device, "phone" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.LiveFeed, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveFeedResponse>>(response);
            _feedPageNumber += 1;

            return _liveAdapter.ConvertToLiveFeedView(result.Data);
        }

        /// <inheritdoc/>
        public async Task<bool> EnterLiveRoomAsync(string roomId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.RoomId, roomId.ToString() },
                { Query.ActionKey, Query.AppKey },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Live.EnterRoom, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse>(response);
            if (data.IsSuccess())
            {
                ConnectLiveSocket();
                if (_liveConnectionTask != null)
                {
                    await _liveConnectionTask.ContinueWith(async result =>
                    {
                        if (result.IsCompleted)
                        {
                            await SendLiveMessageAsync(
                                new
                                {
                                    roomid = roomId,
                                    uid = _accountProvider.UserId,
                                },
                                7);
                        }
                    });
                }

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Models.Data.Community.Partition>> GetLiveAreaIndexAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Device, "phone" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.LiveArea, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveAreaResponse>>(response);

            return result.Data.List.Select(p => _communityAdapter.ConvertToPartition(p));
        }

        /// <inheritdoc/>
        public async Task<LivePartitionView> GetLiveAreaDetailAsync(string areaId, string parentId, string sortType)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Page, _partitionPageNumber.ToString() },
                { Query.PageSizeUnderline, "40" },
                { Query.AreaId, areaId.ToString() },
                { Query.ParentAreaId, parentId.ToString() },
                { Query.Device, "phone" },
            };

            if (!string.IsNullOrEmpty(sortType))
            {
                queryParameters.Add(Query.SortType, sortType);
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.AreaDetail, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveAreaDetailResponse>>(response);
            var data = _liveAdapter.ConvertToLivePartitionView(result.Data);
            data.Id = areaId.ToString();
            _partitionPageNumber += 1;

            return data;
        }

        /// <inheritdoc/>
        public async Task<LiveMediaInformation> GetLiveMediaInformationAsync(string roomId, int quality, bool audioOnly)
        {
            var queryParameter = new Dictionary<string, string>
            {
                { Query.RoomId, roomId.ToString() },
                { Query.NoPlayUrl, "0" },
                { Query.Qn, quality.ToString() },
                { Query.Codec, Uri.EscapeDataString("0,1") },
                { Query.Device, "phone" },
                { Query.DeviceName, Uri.EscapeDataString("iPhone 13") },
                { Query.Dolby, "1" },
                { Query.Format, Uri.EscapeDataString("0,2") },
                { Query.Http, "1" },
                { Query.OnlyAudio, audioOnly ? "1" : "0" },
                { Query.OnlyVideo, "0" },
                { Query.Protocol, Uri.EscapeDataString("0,1") },
                { Query.NeedHdr, "0" },
                { Query.Mask, "0" },
                { Query.PlayType, "0" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.AppPlayInformation, queryParameter, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveAppPlayInformation>>(response);
            return _liveAdapter.ConvertToLiveMediaInformation(result.Data);
        }

        /// <inheritdoc/>
        public async Task<LivePlayerView> GetLiveRoomDetailAsync(string roomId)
        {
            var queryParameter = new Dictionary<string, string>
            {
                { Query.RoomId, roomId },
                { Query.Device, "phone" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Live.RoomDetail, queryParameter, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<LiveRoomDetail>>(response);
            return _liveAdapter.ConvertToLivePlayerView(result.Data);
        }

        /// <inheritdoc/>
        public async Task SendHeartBeatAsync()
        {
            if (_isLiveSocketConnected)
            {
                await SendLiveMessageAsync(string.Empty, 2);
            }
        }

        /// <inheritdoc/>
        public async Task<bool> SendDanmakuAsync(string roomId, string message, string color, bool isStandardSize, DanmakuLocation location)
        {
            var queryParameter = new Dictionary<string, string>
            {
                { Query.Cid, roomId.ToString() },
                { Query.MyId, _accountProvider.UserId.ToString() },
                { Query.MessageSlim, message },
                { Query.Rnd, DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds().ToString() },
                { Query.Mode, ((int)location).ToString() },
                { Query.Pool, "0" },
                { Query.Type, "json" },
                { Query.Color, color },
                { Query.FontSize, isStandardSize ? "25" : "18" },
                { Query.PlayTime, "0.0" },
            };

            try
            {
                var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Live.SendMessage, queryParameter, needToken: true);
                var response = await _httpProvider.SendAsync(request);
                var result = await _httpProvider.ParseAsync<ServerResponse>(response);
                return result.IsSuccess();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void ResetPartitionDetailState()
            => _partitionPageNumber = 1;

        /// <inheritdoc/>
        public void ResetFeedState()
            => _feedPageNumber = 1;

        /// <inheritdoc/>
        public void ResetLiveConnection()
        {
            if (_liveCancellationToken != null)
            {
                _liveCancellationToken.Cancel();
            }

            _liveCancellationToken = new CancellationTokenSource();

            _isLiveSocketConnected = false;
            _liveWebSocket?.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, string.Empty);
            _liveWebSocket?.Dispose();
            _liveWebSocket = null;
        }
    }
}
