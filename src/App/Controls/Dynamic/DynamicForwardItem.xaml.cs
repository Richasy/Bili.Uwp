// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bilibili.App.Dynamic.V2;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 转发动态条目.
    /// </summary>
    public sealed partial class DynamicForwardItem : UserControl, IDynamicLayoutItem
    {
        /// <summary>
        /// <see cref="Data"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(MdlDynForward), typeof(DynamicForwardItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// <see cref="Orientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(DynamicForwardItem), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicForwardItem"/> class.
        /// </summary>
        public DynamicForwardItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 数据.
        /// </summary>
        public MdlDynForward Data
        {
            get { return (MdlDynForward)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// 子项的布局方式.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// 获取播放视图模型.
        /// </summary>
        /// <returns>播放视图模型.</returns>
        public object GetPlayViewModel() => Presenter.GetPlayViewModel();

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DynamicForwardItem;
            if (e.NewValue is MdlDynForward data)
            {
                var item = data.Item;
                var userModule = item.Modules.Where(p => p.ModuleType == DynModuleType.ModuleAuthorForward).FirstOrDefault()?.ModuleAuthorForward;
                var descModule = item.Modules.Where(p => p.ModuleType == DynModuleType.ModuleDesc).FirstOrDefault()?.ModuleDesc;

                if (userModule != null)
                {
                    instance.UserLink.Content = userModule.Title.FirstOrDefault()?.Text ?? "--";
                    instance.UserLink.DataContext = new UserViewModel(Convert.ToInt32(userModule.Uid));
                }

                if (descModule != null)
                {
                    instance.MainContainer.Margin = string.IsNullOrEmpty(descModule.Text.Trim())
                        ? new Thickness(0, -36, 0, 0)
                        : new Thickness(0);
                    instance.DescriptionBlock.DynamicDescription = descModule;
                }
                else
                {
                    instance.MainContainer.Margin = new Thickness(0, -36, 0, 0);
                    instance.DescriptionBlock.Reset();
                }

                instance.Presenter.Data = item;
            }
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DynamicForwardItem;
            if (e.NewValue is Orientation orientation)
            {
                instance.Presenter.ChangeOrientation(orientation);
            }
        }

        private async void OnUserLinkClickAsync(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement).DataContext is UserViewModel data)
            {
                await UserView.Instance.ShowAsync(data);
            }
        }
    }
}
