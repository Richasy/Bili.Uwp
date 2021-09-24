// Copyright (c) GodLeaveMe. All rights reserved.

using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 适用于网络请求的视图模型基类.
    /// </summary>
    public class WebRequestViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebRequestViewModelBase"/> class.
        /// </summary>
        public WebRequestViewModelBase()
        {
            Controller = BiliController.Instance;

            ServiceLocator.Instance.LoadService(out IResourceToolkit resourceToolkit)
                                   .LoadService(out INumberToolkit numberToolkit)
                                   .LoadService(out ISettingsToolkit settingsToolkit);

            ResourceToolkit = resourceToolkit;
            NumberToolkit = numberToolkit;
            SettingsToolkit = settingsToolkit;
        }

        /// <summary>
        /// 是否已经完成了初始化请求.
        /// </summary>
        [Reactive]
        public bool IsRequested { get; set; }

        /// <summary>
        /// 是否在执行初始化数据加载.
        /// </summary>
        [Reactive]
        public bool IsInitializeLoading { get; set; }

        /// <summary>
        /// 是否在执行增量加载.
        /// </summary>
        [Reactive]
        public bool IsDeltaLoading { get; set; }

        /// <summary>
        /// 是否显示错误信息.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }

        /// <summary>
        /// 哔哩控制器.
        /// </summary>
        protected BiliController Controller { get; }

        /// <summary>
        /// 资源管理工具.
        /// </summary>
        protected IResourceToolkit ResourceToolkit { get; }

        /// <summary>
        /// 数字处理工具.
        /// </summary>
        protected INumberToolkit NumberToolkit { get; }

        /// <summary>
        /// 设置管理工具.
        /// </summary>
        protected ISettingsToolkit SettingsToolkit { get; }
    }
}
