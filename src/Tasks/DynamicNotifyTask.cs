// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bili.Controller.Uwp;
using Bili.Locator.Uwp;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bilibili.App.Dynamic.V2;
using Microsoft.Toolkit.Uwp.Notifications;
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
            var controller = BiliController.Instance;
            var dynamics = await controller.GetLatestDynamicVideoListAsync();
            if (dynamics == null)
            {
                def.Complete();
            }

            var settingsToolkit = ServiceLocator.Instance.GetService<ISettingsToolkit>();
            var isFirstCheck = settingsToolkit.ReadLocalSetting(SettingNames.IsFirstRunDynamicNotifyTask, true);
            var cardList = dynamics.DynamicList.List.Where(p =>
                p.CardType == DynamicType.Av
                || p.CardType == DynamicType.Pgc
                || p.CardType == DynamicType.UgcSeason).ToList();
            var firstCard = cardList.First();
            var lastReadId = settingsToolkit.ReadLocalSetting(SettingNames.LastReadVideoDynamicId, string.Empty);

            // 初次检查或者未更新时不会进行通知
            if (isFirstCheck || lastReadId == firstCard.Extend.DynIdStr)
            {
                settingsToolkit.WriteLocalSetting(SettingNames.IsFirstRunDynamicNotifyTask, false);
                settingsToolkit.WriteLocalSetting(SettingNames.LastReadVideoDynamicId, firstCard.Extend.DynIdStr);
                def.Complete();
                return;
            }

            var lastReadCard = cardList.FirstOrDefault(p => p.Extend.DynIdStr == lastReadId);
            var lastReadIndex = cardList.IndexOf(lastReadCard);
            var notifyCards = new List<DynamicItem>();

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
                settingsToolkit.WriteLocalSetting(SettingNames.LastReadVideoDynamicId, firstCard.Extend.DynIdStr);

                foreach (var item in notifyCards)
                {
                    if (sendedCount > 2)
                    {
                        break;
                    }

                    var modules = item.Modules;
                    var userModule = modules.Where(p => p.ModuleType == DynModuleType.ModuleAuthor).FirstOrDefault()?.ModuleAuthor;
                    var descModule = modules.Where(p => p.ModuleType == DynModuleType.ModuleDesc).FirstOrDefault()?.ModuleDesc;
                    var mainModule = modules.Where(p => p.ModuleType == DynModuleType.ModuleDynamic).FirstOrDefault()?.ModuleDynamic;

                    var pgc = mainModule.DynPgc;
                    var video = mainModule.DynArchive;

                    var title = mainModule.ModuleItemCase == ModuleDynamic.ModuleItemOneofCase.DynArchive
                        ? video.Title
                        : pgc.Title;
                    var cover = mainModule.ModuleItemCase == ModuleDynamic.ModuleItemOneofCase.DynArchive
                        ? video.Cover
                        : pgc.Cover;
                    var type = mainModule.ModuleItemCase == ModuleDynamic.ModuleItemOneofCase.DynArchive
                        && video != null
                        ? "video"
                        : "episode";
                    var id = type == "video"
                        ? video.Avid
                        : video.EpisodeId;
                    var avatar = userModule != null
                        ? userModule.Author.Face
                        : "ms-appx:///Assets/Bili_rgba_80.png";
                    var desc = descModule != null
                        ? descModule.Text
                        : userModule.Author.Name;
                    var timeLabel = userModule != null
                        ? userModule.PtimeLabelText
                        : "ms-resource:AppName";

                    cover += "@400w_250h_1c_100q.jpg";
                    avatar += "@100w_100h_1c_100q.jpg";
                    var protocol = $"richasy-bili://play?{type}={id}";
                    if (video != null && video.IsPGC)
                    {
                        protocol += "&isPgc=true";
                    }

                    new ToastContentBuilder()
                        .AddText(title, hintWrap: true, hintMaxLines: 2)
                        .AddText(desc, AdaptiveTextStyle.Caption)
                        .AddAttributionText(timeLabel)
                        .AddAppLogoOverride(new Uri(avatar), ToastGenericAppLogoCrop.Circle)
                        .AddHeroImage(new Uri(cover))
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
