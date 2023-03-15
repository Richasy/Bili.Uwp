// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.App.Args;
using Bili.Models.Data.Appearance;
using Bili.Models.Enums.App;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 负责跨组件呼叫以显示一些提示或UI组件的视图模型.
    /// </summary>
    public sealed partial class CallerViewModel : ViewModelBase, ICallerViewModel
    {
        /// <inheritdoc/>
        public void ShowTip(string message, InfoType type = InfoType.Information)
            => RequestShowTip?.Invoke(this, new AppTipNotificationEventArgs(message, type));

        /// <inheritdoc/>
        public void ShowUpdateDialog(UpdateEventArgs args)
            => RequestShowUpdateDialog?.Invoke(this, args);

        /// <inheritdoc/>
        public void ShowContinuePlayDialog()
            => RequestContinuePlay?.Invoke(this, EventArgs.Empty);

        /// <inheritdoc/>
        public void ShowImages(IEnumerable<Image> images, int firstIndex)
        {
            if (images == null)
            {
                RequestShowImages?.Invoke(this, null);
            }
            else
            {
                RequestShowImages?.Invoke(this, new ShowImageEventArgs(images, firstIndex));
            }
        }

        /// <inheritdoc/>
        public void ShowPgcPlaylist(IPgcPlaylistViewModel vm)
            => RequestShowPgcPlaylist?.Invoke(this, vm);

        /// <inheritdoc/>
        public void ShowArticleReader(IArticleItemViewModel article)
            => RequestShowArticleReader?.Invoke(this, article);

        /// <inheritdoc/>
        public void ShowReply(ShowCommentEventArgs args)
            => RequestShowReplyDetail?.Invoke(this, args);

        /// <inheritdoc/>
        public void ShowPgcSeasonDetail()
            => RequestShowPgcSeasonDetail.Invoke(this, EventArgs.Empty);
    }
}
