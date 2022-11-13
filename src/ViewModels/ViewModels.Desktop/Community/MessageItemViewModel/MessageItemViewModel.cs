// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;
using Humanizer;
using Windows.System;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 消息条目视图模型.
    /// </summary>
    public sealed partial class MessageItemViewModel : ViewModelBase, IMessageItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItemViewModel"/> class.
        /// </summary>
        public MessageItemViewModel(
            ICallerViewModel callerViewModel,
            IResourceToolkit resourceToolkit)
        {
            _callerViewModel = callerViewModel;
            _resourceToolkit = resourceToolkit;
            ActiveCommand = new AsyncRelayCommand(ActiveAsync);
        }

        /// <inheritdoc/>
        public void InjectData(MessageInformation information)
        {
            Data = information;
            PublishTime = information.PublishTime.Humanize();
        }

        private async Task ActiveAsync()
        {
            var sourceId = Data.SourceId;
            if (string.IsNullOrEmpty(sourceId))
            {
                return;
            }

            // 这表示应用暂时不处理此源，而是直接打开网页显示内容.
            if (sourceId.StartsWith("http"))
            {
                await Launcher.LaunchUriAsync(new Uri(sourceId));
            }
            else if (Data.Type == MessageType.Reply)
            {
                // 显示回复信息.
                var type = CommentType.None;
                var isParseFaield = false;
                try
                {
                    type = (CommentType)Convert.ToInt32(Data.Properties["type"]);
                }
                catch (Exception)
                {
                    isParseFaield = true;
                }

                if (isParseFaield || type == CommentType.None)
                {
                    _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.NotSupportReplyType), InfoType.Warning);
                    return;
                }

                var args = new ShowCommentEventArgs(type, CommentSortType.Time, sourceId);
                _callerViewModel.ShowReply(args);
            }
        }
    }
}
