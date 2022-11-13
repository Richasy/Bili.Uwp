// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Community;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Desktop.Common
{
    /// <summary>
    /// 横幅视图模型.
    /// </summary>
    public partial class BannerViewModel
    {
        [ObservableProperty]
        private string _uri;

        [ObservableProperty]
        private string _cover;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private bool _isTooltipEnabled;

        [ObservableProperty]
        private double _minHeight;

        [ObservableProperty]
        private BannerIdentifier _data;
    }
}
