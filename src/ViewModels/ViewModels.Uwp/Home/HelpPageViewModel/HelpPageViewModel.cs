// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Other;
using Bili.Models.Data.Local;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Newtonsoft.Json;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Home
{
    /// <summary>
    /// 帮助支持的视图模型.
    /// </summary>
    public sealed partial class HelpPageViewModel : ViewModelBase, IInitializeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpPageViewModel"/> class.
        /// </summary>
        public HelpPageViewModel(
            IFileToolkit fileToolkit,
            IAppToolkit appToolkit)
        {
            _appToolkit = appToolkit;
            _fileToolkit = fileToolkit;

            QuestionCollection = new ObservableCollection<QuestionModule>();
            LinkCollection = new ObservableCollection<KeyValue<string>>();
            InitializeLinks();

            AskIssueCommand = ReactiveCommand.CreateFromTask(AskIssueAsync, outputScheduler: RxApp.MainThreadScheduler);
            GotoProjectHomeCommand = ReactiveCommand.CreateFromTask(GotoProjectHomeAsync, outputScheduler: RxApp.MainThreadScheduler);
            GotoDeveloperBiliBiliHomePageCommand = ReactiveCommand.CreateFromTask(GotoDeveloperBiliBiliHomePageAsync, outputScheduler: RxApp.MainThreadScheduler);
            InitializeCommand = ReactiveCommand.CreateFromTask(InitializeQuestionsAsync, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 初始化问题.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task InitializeQuestionsAsync()
        {
            if (QuestionCollection.Count > 0)
            {
                return;
            }

            var languageCode = _appToolkit.GetLanguageCode(true);

            // 临时的方法，如果以后可以支持更多的语言，则会移除.
            if (!languageCode.Equals("CHS", System.StringComparison.CurrentCultureIgnoreCase))
            {
                languageCode = "CHS";
            }

            var dataStr = await _fileToolkit.ReadPackageFile($"ms-appx:///Resources/Misc/Question.{languageCode.ToUpper()}.json");
            if (!string.IsNullOrEmpty(dataStr))
            {
                var data = JsonConvert.DeserializeObject<List<QuestionModule>>(dataStr);
                data.ForEach(p => QuestionCollection.Add(p));
                await Task.Delay(100);
                CurrentQuestionModule = QuestionCollection.FirstOrDefault();
            }
        }

        private void InitializeLinks()
        {
            var links = new List<KeyValue<string>>
            {
                new KeyValue<string>("Windows UI Library", "https://github.com/microsoft/microsoft-ui-xaml"),
                new KeyValue<string>("Win2D", "https://github.com/microsoft/Win2D"),
                new KeyValue<string>("Windows Community Toolkit", "https://github.com/CommunityToolkit/WindowsCommunityToolkit"),
                new KeyValue<string>("ReactiveUI", "https://www.reactiveui.net/"),
                new KeyValue<string>("Fluent UI System Icons", "https://github.com/microsoft/fluentui-system-icons"),
                new KeyValue<string>("寒霜弹幕使", "https://github.com/cotaku/DanmakuFrostMaster"),
                new KeyValue<string>("NSDanmaku", "https://github.com/xiaoyaocz/NSDanmaku"),
                new KeyValue<string>("HN.Controls.ImageEx", "https://github.com/h82258652/HN.Controls.ImageEx"),
                new KeyValue<string>("bilibili-API-collect", "https://github.com/SocialSisterYi/bilibili-API-collect"),
                new KeyValue<string>("FFmpeg", "https://github.com/FFmpeg/FFmpeg"),
                new KeyValue<string>("FFmpegInteropX", "https://github.com/ffmpeginteropx/FFmpegInteropX"),
                new KeyValue<string>("BBDown", "https://github.com/nilaoda/BBDown"),
                new KeyValue<string>("哔哩漫游", "https://github.com/yujincheng08/BiliRoaming"),
            };

            links.ForEach(p => LinkCollection.Add(p));
        }

        private async Task AskIssueAsync()
            => await Launcher.LaunchUriAsync(new Uri("https://github.com/Richasy/Bili.Uwp/issues/new/choose")).AsTask();

        private async Task GotoProjectHomeAsync()
            => await Launcher.LaunchUriAsync(new Uri("https://github.com/Richasy/Bili.Uwp/")).AsTask();

        private async Task GotoDeveloperBiliBiliHomePageAsync()
            => await Launcher.LaunchUriAsync(new Uri("https://space.bilibili.com/5992670")).AsTask();
    }
}
