// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.Bili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 评论回复视图模型的基类.
    /// </summary>
    public abstract class ReplyViewModelBase : WebRequestViewModelBase, IDeltaRequestViewModel
    {
        /// <summary>
        /// 目标Id.
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// 评论区类型.
        /// </summary>
        public ReplyType Type { get; set; }

        /// <summary>
        /// 当前排序模式.
        /// </summary>
        [Reactive]
        public Mode CurrentMode { get; set; }

        /// <summary>
        /// 评论集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<ReplyInfo> ReplyCollection { get; set; }

        /// <summary>
        /// 是否显示为空.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <inheritdoc/>
        public virtual Task InitializeRequestAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public virtual Task RequestDataAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 初始化标题.
        /// </summary>
        protected void InitTitle()
        {
            Title = CurrentMode == Mode.MainListHot ?
                    ResourceToolkit.GetLocaleString(LanguageNames.HotReply) :
                    ResourceToolkit.GetLocaleString(LanguageNames.LastestReply);
        }
    }
}
