// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private string LimitAvatar(string avatarUrl)
        {
            return avatarUrl + $"@60w_60h_1c_100q.jpg";
        }
    }
}
