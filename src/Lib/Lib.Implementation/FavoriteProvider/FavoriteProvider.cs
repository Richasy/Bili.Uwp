// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Article;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib
{
    /// <summary>
    /// 收藏夹相关服务提供工具.
    /// </summary>
    public sealed partial class FavoriteProvider : IFavoriteProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        /// <param name="articleAdapter">文章数据适配器.</param>
        /// <param name="favoriteAdapter">收藏夹数据适配器.</param>
        /// <param name="pgcAdapter">PGC数据适配器.</param>
        public FavoriteProvider(
            IHttpProvider httpProvider,
            IFavoriteAdapter favoriteAdapter,
            IArticleAdapter articleAdapter,
            IPgcAdapter pgcAdapter)
        {
            _httpProvider = httpProvider;
            _articleAdapter = articleAdapter;
            _favoriteAdapter = favoriteAdapter;
            _pgcAdapter = pgcAdapter;
        }

        /// <inheritdoc/>
        public async Task<(VideoFavoriteSet, IEnumerable<string>)> GetCurrentPlayerFavoriteListAsync(string userId, string videoId)
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
            var data = result.Data;
            var count = data.Count;
            var items = data.List.Select(p => _favoriteAdapter.ConvertToVideoFavoriteFolder(p));
            var ids = data.List.Where(p => p.FavoriteState == 1).Select(p => p.Id.ToString());
            var favoriteSet = new VideoFavoriteSet(items, count);
            return (favoriteSet, ids);
        }

        /// <inheritdoc/>
        public async Task<VideoFavoriteView> GetVideoFavoriteViewAsync(string userId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.UpId, userId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.VideoFavoriteGallery, queryParameters, Models.Enums.RequestClientType.IOS, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<VideoFavoriteGalleryResponse>>(response);
            _videoCollectFolderPageNumber = 1;
            _videoCreatedFolderPageNumber = 1;
            return _favoriteAdapter.ConvertToVideoFavoriteView(result.Data);
        }

        /// <inheritdoc/>
        public async Task<VideoFavoriteFolderDetail> GetVideoFavoriteFolderDetailAsync(string folderId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.MediaId, folderId },
                { Query.PageSizeSlim, "20" },
                { Query.PageNumber, _videoFolderDetailPageNumber.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.VideoFavoriteDelta, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<VideoFavoriteListResponse>>(response);
            _videoFolderDetailPageNumber++;
            return _favoriteAdapter.ConvertToVideoFavoriteFolderDetail(result.Data);
        }

        /// <inheritdoc/>
        public async Task<VideoFavoriteSet> GetVideoFavoriteFolderListAsync(string userId, bool isCreated)
        {
            var pn = isCreated ? _videoCreatedFolderPageNumber : _videoCollectFolderPageNumber;
            var queryParameters = new Dictionary<string, string>
            {
                { Query.UpId, userId.ToString() },
                { Query.PageSizeSlim, "20" },
                { Query.PageNumber, pn.ToString() },
            };

            var url = isCreated ? Account.CreatedVideoFavoriteFolderDelta : Account.CollectedVideoFavoriteFolderDelta;
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, url, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<FavoriteMediaList>>(response);
            if (isCreated)
            {
                _videoCreatedFolderPageNumber++;
            }
            else
            {
                _videoCollectFolderPageNumber++;
            }

            var count = result.Data.Count;
            var favorites = result.Data.List.Select(p => _favoriteAdapter.ConvertToVideoFavoriteFolder(p));
            return new VideoFavoriteSet(favorites, count);
        }

        /// <inheritdoc/>
        public async Task<SeasonSet> GetFavoriteAnimeListAsync(int status)
        {
            var data = await GetPgcFavoriteListInternalAsync(Account.AnimeFavorite, _animeFolderPageNumber, status);
            _animeFolderPageNumber++;
            return data;
        }

        /// <inheritdoc/>
        public async Task<SeasonSet> GetFavoriteCinemaListAsync(int status)
        {
            var data = await GetPgcFavoriteListInternalAsync(Account.CinemaFavorite, _cinemaFolderPageNumber, status);
            _cinemaFolderPageNumber++;
            return data;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateFavoritePgcStatusAsync(string seasonId, int status)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.SeasonId, seasonId },
                { Query.Status, status.ToString() },
                { Query.Device, "phone" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.UpdatePgcStatus, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.Message == "success";
        }

        /// <inheritdoc/>
        public async Task<ArticleSet> GetFavortieArticleListAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PageNumber, _articleFolderPageNumber.ToString() },
                { Query.PageSizeSlim, "20" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Account.ArticleFavorite, queryParameters, needToken: true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<ArticleFavoriteListResponse>>(response);
            return _articleAdapter.ConvertToArticleSet(result.Data);
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFavoriteFolderAsync(string favoriteId, bool isMe)
        {
            var queryParameters = new Dictionary<string, string>();
            string uri;
            if (isMe)
            {
                uri = Account.DeleteFavoriteFolder;
                queryParameters.Add(Query.MediaIds, favoriteId);
            }
            else
            {
                uri = Account.UnFavoriteFolder;
                queryParameters.Add(Query.MediaId, favoriteId);
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, uri, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFavoriteVideoAsync(string favoriteId, string videoId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.MediaId, favoriteId },
                { Query.Resources, $"{videoId}:2" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.UnFavoriteVideo, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFavoritePgcAsync(string seasonId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.SeasonId, seasonId },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.UnFavoritePgc, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveFavoriteArticleAsync(string articleId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Id, articleId },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Account.UnFavoriteArticle, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public void ResetVideoFolderDetailStatus()
            => _videoFolderDetailPageNumber = 1;

        /// <inheritdoc/>
        public void ResetAnimeStatus()
            => _animeFolderPageNumber = 1;

        /// <inheritdoc/>
        public void ResetCinemaStatus()
            => _cinemaFolderPageNumber = 1;

        /// <inheritdoc/>
        public void ResetArticleStatus()
            => _articleFolderPageNumber = 1;
    }
}
