// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 授权模块的属性集及扩展.
    /// </summary>
    public partial class AuthorizeProvider
    {
        private AuthorizeState _state;
        private readonly IMD5Toolkit _md5Toolkit;
        private string _accessToken;
    }
}
