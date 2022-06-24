// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播播放页视图模型.
    /// </summary>
    public sealed partial class LivePlayerPageViewModel
    {
        /// <inheritdoc/>
        public void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = GetErrorMessage(exception);
            ErrorText = $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestLiveFailed)}\n{msg}";
            LogException(exception);
        }
    }
}
