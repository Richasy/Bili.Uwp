// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 动态展示.
    /// </summary>
    public sealed partial class DynamicPresenter : UserControl
    {
        /// <summary>
        /// <see cref="Data"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(Bilibili.App.Dynamic.V2.DynamicItem), typeof(DynamicPresenter), new PropertyMetadata(null, new PropertyChangedCallback(DataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPresenter"/> class.
        /// </summary>
        public DynamicPresenter()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 数据.
        /// </summary>
        public Bilibili.App.Dynamic.V2.DynamicItem Data
        {
            get { return (Bilibili.App.Dynamic.V2.DynamicItem)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        /// <summary>
        /// 更改布局方式.
        /// </summary>
        /// <param name="orientation">布局方向.</param>
        public void ChangeOrientation(Orientation orientation)
        {
            if (VisualTreeHelper.GetChildrenCount(MainPresenter) > 0)
            {
                var child = VisualTreeHelper.GetChild(MainPresenter, 0);
                if (child is IDynamicLayoutItem item)
                {
                    item.Orientation = orientation;
                }
            }
        }

        /// <summary>
        /// 获取可供播放的视图模型.
        /// </summary>
        /// <returns>播放视图模型.</returns>
        public object GetPlayViewModel()
        {
            if (MainPresenter.Content is VideoViewModel videoVM)
            {
                return videoVM;
            }
            else if (MainPresenter.Content is SeasonViewModel seasonVM)
            {
                return seasonVM;
            }
            else if (MainPresenter.Content is Bilibili.App.Dynamic.V2.MdlDynForward)
            {
                var forwardItem = MainPresenter.FindDescendantElementByType<DynamicForwardItem>();
                return forwardItem.GetPlayViewModel();
            }

            return null;
        }

        private static void DataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DynamicPresenter;
            var data = e.NewValue as Bilibili.App.Dynamic.V2.DynamicItem;
            var mainModule = data.Modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleDynamic).FirstOrDefault()?.ModuleDynamic;

            instance.Visibility = mainModule == null ? Visibility.Collapsed : Visibility.Visible;
            if (mainModule == null)
            {
                return;
            }

            if (mainModule.Type == Bilibili.App.Dynamic.V2.ModuleDynamicType.MdlDynPgc)
            {
                instance.MainPresenter.Content = new SeasonViewModel(mainModule.DynPgc);
                instance.MainPresenter.ContentTemplate = instance.PgcTemplate;
            }
            else if (mainModule.Type == Bilibili.App.Dynamic.V2.ModuleDynamicType.MdlDynArchive)
            {
                var userModule = data.Modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleAuthor).FirstOrDefault()?.ModuleAuthor;

                UserViewModel publisher = null;
                if (userModule != null)
                {
                    publisher = new UserViewModel(userModule.Author.Name, userModule.Author.Face, Convert.ToInt32(userModule.Mid));
                }

                var vm = new VideoViewModel(mainModule.DynArchive)
                {
                    Publisher = publisher,
                };
                instance.MainPresenter.Content = vm;
                instance.MainPresenter.ContentTemplate = instance.VideoTemplate;
            }
            else if (mainModule.Type == Bilibili.App.Dynamic.V2.ModuleDynamicType.MdlDynForward)
            {
                instance.MainPresenter.Content = mainModule.DynForward;
                instance.MainPresenter.ContentTemplate = instance.ForwardTemplate;
            }
            else if (mainModule.Type == Bilibili.App.Dynamic.V2.ModuleDynamicType.MdlDynDraw)
            {
                instance.MainPresenter.Content = mainModule.DynDraw;
                instance.MainPresenter.ContentTemplate = instance.ImageTemplate;
            }
            else
            {
                instance.MainPresenter.Content = mainModule;
                instance.MainPresenter.ContentTemplate = instance.NoneTemplate;
            }
        }
    }
}
