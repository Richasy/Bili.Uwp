﻿// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Workspace;
using CommunityToolkit.Mvvm.Input;
using Models.Workspace;

namespace Bili.ViewModels.Workspace.Pages
{
    /// <summary>
    /// 首页视图模型.
    /// </summary>
    public sealed partial class HomePageViewModel : ViewModelBase, IHomePageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomePageViewModel"/> class.
        /// </summary>
        public HomePageViewModel(
            ISettingsToolkit settingsToolkit,
            IResourceToolkit resourceToolkit,
            IHomeProvider homeProvider)
        {
            _settingsToolkit = settingsToolkit;
            _resourceToolkit = resourceToolkit;
            _homeProvider = homeProvider;
            FixedVideoPartitions = new ObservableCollection<Partition>();
            VideoPartitions = new ObservableCollection<IVideoPartitionViewModel>();
            Topics = new ObservableCollection<QuickTopic>
            {
                CreateTopic(LanguageNames.Anime, "anime", "anime", "anime"),
                CreateTopic(LanguageNames.DomesticAnime, "domesticAnime", "guochuang", "guochuang"),
                CreateTopic(LanguageNames.SpecialColumn, "specialColumn", "read/home", "document"),
                CreateTopic(LanguageNames.Documentary, "documentary", "documentary", "jilu"),
                CreateTopic(LanguageNames.Movie, "movie", "movie", "movie"),
                CreateTopic(LanguageNames.TV, "tv", "tv", "tv"),
            };

            AttachExceptionHandlerToAsyncCommand(LogException, InitializeVideoPartitionsCommand);
            AttachIsRunningToAsyncCommand(p => IsVideoPartitionLoading = p, InitializeVideoPartitionsCommand);
        }

        [RelayCommand]
        private async Task InitializeVideoPartitionsAsync()
        {
            TryClear(VideoPartitions);
            TryClear(FixedVideoPartitions);
            var items = await _homeProvider.GetVideoPartitionIndexAsync();
            var fixedPartitions = _settingsToolkit.ReadLocalSetting(SettingNames.FixedPartitions, string.Empty);
            if (string.IsNullOrEmpty(fixedPartitions))
            {
                fixedPartitions = string.Join(',', items.Take(6).Select(p => p.Id));
            }
            else if (fixedPartitions == "-1")
            {
                return;
            }

            var partitionIds = fixedPartitions.Split(',');
            foreach (var partitionId in partitionIds)
            {
                var part = items.FirstOrDefault(p => p.Id == partitionId);
                if (part != null)
                {
                    FixedVideoPartitions.Add(part);
                }
            }

            foreach (var item in items)
            {
                var vm = Locator.Instance.GetService<IVideoPartitionViewModel>();
                vm.Data = item;
                vm.IsFixed = FixedVideoPartitions.Any(p => p.Id == item.Id);
                vm.InjectAction((isFixed, v) =>
                {
                    if (isFixed)
                    {
                        FixedVideoPartitions.Add(v.Data);
                    }
                    else
                    {
                        FixedVideoPartitions.Remove(v.Data);
                    }

                    var ids = string.Join(',', FixedVideoPartitions.Select(p => p.Id));
                    if (string.IsNullOrEmpty(ids))
                    {
                        ids = "-1";
                    }

                    _settingsToolkit.WriteLocalSetting(SettingNames.FixedPartitions, ids);
                });

                VideoPartitions.Add(vm);
            }
        }

        private QuickTopic CreateTopic(LanguageNames title, string biliPartition, string webName, string iconName)
        {
            return new QuickTopic(
                _resourceToolkit.GetLocaleString(title),
                $"richasy-bili://navigate?id={biliPartition}",
                $"https://www.bilibili.com/{webName}",
                $"ms-appx:///Assets/Images/{iconName}.png");
        }
    }
}
