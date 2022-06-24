// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter;
using Bili.Adapter.Interfaces;
using Bili.Lib;
using Bili.Lib.Interfaces;
using Bili.SignIn.Uwp;
using Bili.Toolkit.Fake;
using Bili.Toolkit.Interfaces;
using Bili.Toolkit.Uwp;
using Splat;

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
            SplatRegistrations.Register<IResourceToolkit, FakeResourceToolkit>();
            SplatRegistrations.Register<ISettingsToolkit, SettingsToolkit>();
            SplatRegistrations.Register<INumberToolkit, NumberToolkit>();
            SplatRegistrations.Register<IMD5Toolkit, MD5Toolkit>();

            SplatRegistrations.Register<IImageAdapter, ImageAdapter>();
            SplatRegistrations.Register<IUserAdapter, UserAdapter>();
            SplatRegistrations.Register<ICommunityAdapter, CommunityAdapter>();
            SplatRegistrations.Register<IVideoAdapter, VideoAdapter>();
            SplatRegistrations.Register<IPgcAdapter, PgcAdapter>();
            SplatRegistrations.Register<IDynamicAdapter, DynamicAdapter>();
            SplatRegistrations.Register<ICommentAdapter, CommentAdapter>();

            SplatRegistrations.Register<IAuthorizeProvider, AuthorizeProvider>();
            SplatRegistrations.Register<IHttpProvider, HttpProvider>();
            SplatRegistrations.Register<IAccountProvider, AccountProvider>();
            SplatRegistrations.Register<ICommunityProvider, CommunityProvider>();
            SplatRegistrations.SetupIOC();
        }
    }
}
