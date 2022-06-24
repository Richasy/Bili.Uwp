// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Humanizer;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 消息条目视图模型.
    /// </summary>
    public sealed partial class MessageItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItemViewModel"/> class.
        /// </summary>
        public MessageItemViewModel(
            AppViewModel appViewModel,
            IResourceToolkit resourceToolkit)
        {
            _appViewModel = appViewModel;
            _resourceToolkit = resourceToolkit;
            ActiveCommand = ReactiveCommand.CreateFromTask(ActiveAsync, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置具体的消息信息.
        /// </summary>
        /// <param name="information">消息信息.</param>
        public void SetInformation(MessageInformation information)
        {
            Information = information;
            PublishTime = information.PublishTime.Humanize();
        }

        private async Task ActiveAsync()
        {
            var sourceId = Information.SourceId;
            if (string.IsNullOrEmpty(sourceId))
            {
                return;
            }

            // 这表示应用暂时不处理此源，而是直接打开网页显示内容.
            if (sourceId.StartsWith("http"))
            {
                await Launcher.LaunchUriAsync(new Uri(sourceId));
            }
            else if (Information.Type == MessageType.Reply)
            {
                // 显示回复信息.
                var type = CommentType.None;
                var isParseFaield = false;
                try
                {
                    type = (CommentType)Convert.ToInt32(Information.Properties["type"]);
                }
                catch (Exception)
                {
                    isParseFaield = true;
                }

                if (isParseFaield || type == CommentType.None)
                {
                    _appViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.NotSupportReplyType), InfoType.Warning);
                    return;
                }

                var args = new ShowCommentEventArgs(type, CommentSortType.Time, sourceId);
                _appViewModel.ShowReply(args);
            }
        }
    }
}
