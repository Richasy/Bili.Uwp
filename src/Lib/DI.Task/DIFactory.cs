// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter;
using Bili.Adapter.Interfaces;
using Bili.Lib;
using Bili.Lib.Interfaces;
using Bili.SignIn.Uwp;
using Bili.Toolkit.Fake;
using Bili.Toolkit.Interfaces;
using Bili.Toolkit.Uwp;

namespace Bili.DI.Task
{
    /// <summary>
    /// 后台任务的依赖注入.
    /// </summary>
    public class DIFactory
    {
        /// <summary>
        /// 注册后台任务所需的服务.
        /// </summary>
        public void RegisterTaskRequiredServices()
        {
            Container.Locator.Instance
                .RegisterSingleton<IResourceToolkit, FakeResourceToolkit>()
                .RegisterSingleton<ISettingsToolkit, SettingsToolkit>()
                .RegisterSingleton<INumberToolkit, NumberToolkit>()
                .RegisterSingleton<IMD5Toolkit, MD5Toolkit>()
                .RegisterSingleton<ITextToolkit, TextToolkit>()
                .RegisterSingleton<IImageAdapter, ImageAdapter>()
                .RegisterSingleton<IUserAdapter, UserAdapter>()
                .RegisterSingleton<ICommunityAdapter, CommunityAdapter>()
                .RegisterSingleton<IVideoAdapter, VideoAdapter>()
                .RegisterSingleton<IPgcAdapter, PgcAdapter>()
                .RegisterSingleton<IArticleAdapter, ArticleAdapter>()
                .RegisterSingleton<IDynamicAdapter, DynamicAdapter>()
                .RegisterSingleton<ICommentAdapter, CommentAdapter>()
                .RegisterSingleton<IAuthorizeProvider, AuthorizeProvider>()
                .RegisterSingleton<IHttpProvider, HttpProvider>()
                .RegisterSingleton<IAccountProvider, AccountProvider>()
                .RegisterSingleton<ICommunityProvider, CommunityProvider>()
                .Build();
        }
    }
}
