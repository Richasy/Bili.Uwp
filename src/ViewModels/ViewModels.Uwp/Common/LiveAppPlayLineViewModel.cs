// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Locator.Uwp;
using Bili.Models.BiliBili;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 应用直播线路视图模型.
    /// </summary>
    public class LiveAppPlayLineViewModel : SelectableViewModelBase<LiveAppPlayUrl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePlayLineViewModel"/> class.
        /// </summary>
        /// <param name="data">播放线路数据.</param>
        /// <param name="baseUrl">基础链接.</param>
        /// <param name="id">标识序号.</param>
        /// <param name="isSelected">是否被选中.</param>
        public LiveAppPlayLineViewModel(LiveAppPlayUrl data, string baseUrl = "", int id = 1, bool isSelected = false)
            : base(data, isSelected)
        {
            Url = $"{data.Host}{baseUrl}{data.Extra}";
            Name = ServiceLocator.Instance.GetService<IResourceToolkit>().GetLocaleString(Models.Enums.LanguageNames.PlayLine) + id;
        }

        /// <summary>
        /// 播放地址.
        /// </summary>
        [Reactive]
        public string Url { get; set; }

        /// <summary>
        /// 名称.
        /// </summary>
        [Reactive]
        public string Name { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LiveAppPlayLineViewModel model && base.Equals(obj) && Url == model.Url;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = -1169443244;
            hashCode = (hashCode * -1521134295) + base.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Url);
            return hashCode;
        }
    }
}
