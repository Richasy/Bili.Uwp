// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Other;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 帮助支持的视图模型.
    /// </summary>
    public partial class HelpViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HelpViewModel"/> class.
        /// </summary>
        protected HelpViewModel()
        {
            ServiceLocator.Instance.LoadService(out _fileToolkit)
                                   .LoadService(out _appToolkit);
            InitializeLinks();
        }

        /// <summary>
        /// 初始化问题.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeQuestionsAsync()
        {
            QuestionCollection = new ObservableCollection<QuestionModule>();
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
                CurrentQuestionModule = QuestionCollection.FirstOrDefault();
            }
        }

        private void InitializeLinks()
        {
            LinkCollection = new ObservableCollection<KeyValue<string>>
            {
                new KeyValue<string>("Windows UI Library", "https://github.com/microsoft/microsoft-ui-xaml"),
                new KeyValue<string>("Win2D", "https://github.com/microsoft/Win2D"),
                new KeyValue<string>("Windows Community Toolkit", "https://github.com/CommunityToolkit/WindowsCommunityToolkit"),
                new KeyValue<string>("Fluent UI System Icons", "https://github.com/microsoft/fluentui-system-icons"),
                new KeyValue<string>("NSDanmaku", "https://github.com/xiaoyaocz/NSDanmaku"),
                new KeyValue<string>("HN.Controls.ImageEx", "https://github.com/xiaoyaocz/NSDanmaku"),
                new KeyValue<string>("bilibili-API-collect", "https://github.com/SocialSisterYi/bilibili-API-collect"),
                new KeyValue<string>("FFmpeg", "https://github.com/FFmpeg/FFmpeg"),
                new KeyValue<string>("FFmpegInteropX", "https://github.com/ffmpeginteropx/FFmpegInteropX"),
            };
        }
    }
}
