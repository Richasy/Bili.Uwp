// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.Adapter.Interfaces;
using Bili.Models.Data.Appearance;
using Bili.Models.Data.Community;
using Bili.Models.Data.Dynamic;
using Bili.Models.Data.User;
using Bili.Models.Enums.Bili;
using Bili.Models.Enums.Community;
using Bilibili.App.Dynamic.V2;

namespace Bili.Adapter
{
    /// <summary>
    /// 动态数据适配器.
    /// </summary>
    public sealed class DynamicAdapter : IDynamicAdapter
    {
        private readonly IUserAdapter _userAdapter;
        private readonly IImageAdapter _imageAdapter;
        private readonly IVideoAdapter _videoAdapter;
        private readonly IPgcAdapter _pgcAdapter;
        private readonly IArticleAdapter _articleAdapter;
        private readonly ICommunityAdapter _communityAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicAdapter"/> class.
        /// </summary>
        /// <param name="userAdapter">用户数据适配器.</param>
        /// <param name="imageAdapter">图片数据适配器.</param>
        /// <param name="videoAdapter">视频数据适配器.</param>
        /// <param name="pgcAdapter">PGC 数据适配器.</param>
        /// <param name="articleAdapter">文章数据适配器.</param>
        /// <param name="communityAdapter">社区数据适配器.</param>
        public DynamicAdapter(
            IUserAdapter userAdapter,
            IImageAdapter imageAdapter,
            IVideoAdapter videoAdapter,
            IPgcAdapter pgcAdapter,
            IArticleAdapter articleAdapter,
            ICommunityAdapter communityAdapter)
        {
            _userAdapter = userAdapter;
            _imageAdapter = imageAdapter;
            _videoAdapter = videoAdapter;
            _pgcAdapter = pgcAdapter;
            _articleAdapter = articleAdapter;
            _communityAdapter = communityAdapter;
        }

        /// <inheritdoc/>
        public DynamicInformation ConvertToDynamicInformation(DynamicItem item)
        {
            var modules = item.Modules;
            var userModule = modules.Where(p => p.ModuleType == DynModuleType.ModuleAuthor).FirstOrDefault()?.ModuleAuthor;
            var descModule = modules.Where(p => p.ModuleType == DynModuleType.ModuleDesc).FirstOrDefault()?.ModuleDesc;
            var mainModule = modules.Where(p => p.ModuleType == DynModuleType.ModuleDynamic).FirstOrDefault()?.ModuleDynamic;
            var dataModule = modules.Where(p => p.ModuleType == DynModuleType.ModuleStat).FirstOrDefault()?.ModuleStat;

            UserProfile user = default;
            string tip = default;
            EmoteText description = default;
            DynamicCommunityInformation communityInfo = default;
            var dynamicId = item.Extend.DynIdStr;
            var replyType = GetReplyTypeFromDynamicType(item.CardType);
            var replyId = replyType == CommentType.Dynamic
                ? dynamicId
                : item.Extend.BusinessId;
            var dynamicType = GetDynamicItemType(mainModule);
            var dynamicData = GetDynamicContent(mainModule);
            if (userModule != null)
            {
                var author = userModule.Author;
                user = _userAdapter.ConvertToUserProfile(Convert.ToInt32(author.Mid), author.Name, author.Face, Models.Enums.App.AvatarSize.Size32);
                tip = userModule.PtimeLabelText;
            }
            else
            {
                var forwardUserModule = modules.Where(p => p.ModuleType == DynModuleType.ModuleAuthorForward).FirstOrDefault()?.ModuleAuthorForward;
                if (forwardUserModule != null)
                {
                    var name = forwardUserModule.Title.FirstOrDefault()?.Text ?? "--";
                    user = _userAdapter.ConvertToUserProfile(Convert.ToInt32(forwardUserModule.Uid), name, forwardUserModule.FaceUrl, Models.Enums.App.AvatarSize.Size32);
                    tip = forwardUserModule.PtimeLabelText;
                }
            }

            if (descModule != null)
            {
                description = _imageAdapter.ConvertToEmoteText(descModule);
            }

            if (dataModule != null)
            {
                communityInfo = _communityAdapter.ConvertToDynamicCommunityInformation(dataModule, dynamicId);
            }

            return new DynamicInformation(
                dynamicId,
                user,
                tip,
                communityInfo,
                replyType,
                replyId,
                dynamicType,
                dynamicData,
                description);
        }

        /// <inheritdoc/>
        public DynamicInformation ConvertToDynamicInformation(MdlDynForward forward)
        {
            var item = forward.Item;
            return ConvertToDynamicInformation(item);
        }

        /// <inheritdoc/>
        public DynamicView ConvertToDynamicView(DynVideoReply reply)
        {
            var list = reply.DynamicList.List.Where(p =>
                p.CardType == DynamicType.Av
                || p.CardType == DynamicType.Pgc
                || p.CardType == DynamicType.UgcSeason)
                .Select(p => ConvertToDynamicInformation(p))
                .ToList();

            return new DynamicView(list);
        }

        /// <inheritdoc/>
        public DynamicView ConvertToDynamicView(DynAllReply reply)
        {
            var list = reply.DynamicList.List.Where(p =>
                p.CardType != DynamicType.DynNone
                && p.CardType != DynamicType.Ad)
                .Select(p => ConvertToDynamicInformation(p))
                .ToList();

            return new DynamicView(list);
        }

        private CommentType GetReplyTypeFromDynamicType(DynamicType type)
        {
            var replyType = CommentType.None;
            switch (type)
            {
                case DynamicType.Forward:
                case DynamicType.Word:
                case DynamicType.Live:
                    replyType = CommentType.Dynamic;
                    break;
                case DynamicType.Draw:
                    replyType = CommentType.Album;
                    break;
                case DynamicType.Av:
                case DynamicType.Pgc:
                case DynamicType.UgcSeason:
                case DynamicType.Medialist:
                    replyType = CommentType.Video;
                    break;
                case DynamicType.Courses:
                case DynamicType.CoursesSeason:
                    replyType = CommentType.Course;
                    break;
                case DynamicType.Article:
                    replyType = CommentType.Article;
                    break;
                case DynamicType.Music:
                    replyType = CommentType.Music;
                    break;
                default:
                    break;
            }

            return replyType;
        }

        private DynamicItemType GetDynamicItemType(ModuleDynamic dynamic)
        {
            if (dynamic == null)
            {
                return DynamicItemType.PlainText;
            }

            var type = dynamic.Type switch
            {
                ModuleDynamicType.MdlDynArchive => dynamic.DynArchive.IsPGC
                    ? DynamicItemType.Pgc
                    : DynamicItemType.Video,
                ModuleDynamicType.MdlDynPgc => DynamicItemType.Pgc,
                ModuleDynamicType.MdlDynForward => DynamicItemType.Forward,
                ModuleDynamicType.MdlDynDraw => DynamicItemType.Image,
                ModuleDynamicType.MdlDynArticle => DynamicItemType.Article,
                _ => DynamicItemType.Unsupported,
            };

            return type;
        }

        private object GetDynamicContent(ModuleDynamic dynamic)
        {
            if (dynamic == null)
            {
                return null;
            }

            if (dynamic.Type == ModuleDynamicType.MdlDynPgc)
            {
                return _pgcAdapter.ConvertToEpisodeInformation(dynamic.DynPgc);
            }
            else if (dynamic.Type == ModuleDynamicType.MdlDynArchive)
            {
                return dynamic.DynArchive.IsPGC
                    ? _pgcAdapter.ConvertToEpisodeInformation(dynamic.DynArchive)
                    : _videoAdapter.ConvertToVideoInformation(dynamic.DynArchive);
            }
            else if (dynamic.Type == ModuleDynamicType.MdlDynForward)
            {
                return ConvertToDynamicInformation(dynamic.DynForward);
            }
            else if (dynamic.Type == ModuleDynamicType.MdlDynDraw)
            {
                return dynamic.DynDraw.Items.Select(p => _imageAdapter.ConvertToImage(p.Src, 100d, 100d)).ToList();
            }
            else if (dynamic.Type == ModuleDynamicType.MdlDynArticle)
            {
                return _articleAdapter.ConvertToArticleInformation(dynamic.DynArticle);
            }

            return null;
        }
    }
}
