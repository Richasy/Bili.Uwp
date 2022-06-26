// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bili.DI.Task;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Dynamic;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Microsoft.Toolkit.Uwp.Notifications;
using Splat;
using Windows.ApplicationModel.Background;

namespace Bili.Tasks
{
    /// <summary>
    /// 动态更新通知.
    /// </summary>
    public sealed class DynamicNotifyTask : IBackgroundTask
    {
        /// <inheritdoc/>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var def = taskInstance.GetDeferral();
            new DIFactory().RegisterTaskRequiredServices();
            var settingsToolkit = Locator.Current.GetService<ISettingsToolkit>();
            var communityProvider = Locator.Current.GetService<ICommunityProvider>();
            var authProvider = Locator.Current.GetService<IAuthorizeProvider>();

            var isSignedIn = await authProvider.IsTokenValidAsync();
            if (!isSignedIn)
            {
                def.Complete();
                return;
            }

            var dynamics = await communityProvider.GetDynamicVideoListAsync();
            if (dynamics == null || dynamics.Dynamics?.Count() == 0)
            {
                def.Complete();
                return;
            }

            var isFirstCheck = settingsToolkit.ReadLocalSetting(SettingNames.IsFirstRunDynamicNotifyTask, true);
            var firstCard = dynamics.Dynamics.First();
            var cardList = dynamics.Dynamics.ToList();

            var lastReadId = settingsToolkit.ReadLocalSetting(SettingNames.LastReadVideoDynamicId, string.Empty);

            // 初次检查或者未更新时不会进行通知
            if (isFirstCheck || lastReadId == firstCard.Id)
            {
                settingsToolkit.WriteLocalSetting(SettingNames.IsFirstRunDynamicNotifyTask, false);
                settingsToolkit.WriteLocalSetting(SettingNames.LastReadVideoDynamicId, firstCard.Id);
                def.Complete();
                return;
            }

            var lastReadCard = cardList.FirstOrDefault(p => p.Id == lastReadId);
            var lastReadIndex = cardList.IndexOf(lastReadCard);
            var notifyCards = new List<DynamicInformation>();

            if (lastReadIndex != -1)
            {
                for (var i = 0; i < lastReadIndex; i++)
                {
                    notifyCards.Add(cardList[i]);
                }
            }
            else
            {
                notifyCards = cardList;
            }

            var sendedCount = 0;

            if (notifyCards.Count > 0)
            {
                settingsToolkit.WriteLocalSetting(SettingNames.LastReadVideoDynamicId, firstCard.Id);

                foreach (var item in notifyCards)
                {
                    if (sendedCount > 2)
                    {
                        break;
                    }

                    var title = string.Empty;
                    var coverUrl = string.Empty;
                    var type = "video";
                    var id = string.Empty;
                    var avatar = item.User.Avatar.GetSourceUri().ToString();
                    var desc = item.Description?.Text;
                    var timeLabel = string.IsNullOrEmpty(item.Tip)
                        ? "ms-resource:AppName"
                        : item.Tip;
                    var additional = string.Empty;

                    if (item.Data is VideoInformation videoInfo)
                    {
                        id = videoInfo.Identifier.Id;
                        title = videoInfo.Identifier.Title;
                        coverUrl = videoInfo.Identifier.Cover.GetSourceUri().ToString();
                        if (string.IsNullOrEmpty(desc))
                        {
                            desc = videoInfo.Publisher?.User?.Name ?? string.Empty;
                        }
                    }
                    else if (item.Data is EpisodeInformation episodeInfo)
                    {
                        id = episodeInfo.VideoId;
                        title = episodeInfo.Identifier.Title;
                        coverUrl = episodeInfo.Identifier.Cover.GetSourceUri().ToString();
                        additional = "&isPgc=true";
                        if (string.IsNullOrEmpty(desc))
                        {
                            desc = episodeInfo.Subtitle;
                        }
                    }

                    coverUrl += "@400w_250h_1c_100q.jpg";
                    avatar += "@100w_100h_1c_100q.jpg";
                    var protocol = $"richasy-bili://play?{type}={id}{additional}";

                    new ToastContentBuilder()
                        .AddText(title, hintWrap: true, hintMaxLines: 2)
                        .AddText(desc, AdaptiveTextStyle.Caption)
                        .AddAttributionText(timeLabel)
                        .AddAppLogoOverride(new Uri(avatar), ToastGenericAppLogoCrop.Circle)
                        .AddHeroImage(new Uri(coverUrl))
                        .SetProtocolActivation(new Uri(protocol))
                        .Show(toast =>
                        {
                            toast.Group = "Bili";
                        });
                    sendedCount++;
                }

                if (notifyCards.Count > sendedCount)
                {
                    // 弹出省略提示
                    new ToastContentBuilder()
                        .AddText("ms-resource:MoreInDynamic")
                        .SetProtocolActivation(new Uri("richasy-bili://navigate?id=Dynamic"))
                        .Show(toast =>
                        {
                            toast.Group = "Bili";
                        });
                }
            }

            def.Complete();
        }
    }
}
