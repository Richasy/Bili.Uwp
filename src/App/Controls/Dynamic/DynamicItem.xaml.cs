// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 动态条目.
    /// </summary>
    public sealed partial class DynamicItem : UserControl
    {
        /// <summary>
        /// <see cref="Data"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(Bilibili.App.Dynamic.V2.DynamicItem), typeof(DynamicItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItem"/> class.
        /// </summary>
        public DynamicItem()
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

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Bilibili.App.Dynamic.V2.DynamicItem data)
            {
                var instance = d as DynamicItem;
                var modules = data.Modules.ToList();
                var userModule = modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleAuthor).FirstOrDefault()?.ModuleAuthor;
                var descModule = modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleDesc).FirstOrDefault()?.ModuleDesc;
                var mainModule = modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleDynamic).FirstOrDefault()?.ModuleDynamic;
                var dataModule = modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleStat).FirstOrDefault()?.ModuleStat;
                var inteModule = modules.Where(p => p.ModuleType == Bilibili.App.Dynamic.V2.DynModuleType.ModuleInteraction).FirstOrDefault()?.ModuleInteraction;

                if (userModule != null)
                {
                    instance.UserAvatar.Avatar = userModule.Author.Face;
                    instance.UserAvatar.UserName = userModule.Author.Name;
                    instance.UserNameBlock.Text = userModule.Author.Name;
                    instance.DateBlock.Text = userModule.PtimeLabelText;
                }

                if (descModule != null)
                {
                    if (string.IsNullOrEmpty(descModule.Text.Trim()))
                    {
                        instance.MainContainer.Margin = new Thickness(0, -36, 0, 0);
                    }
                    else
                    {
                        instance.MainContainer.Margin = new Thickness(0);
                    }

                    instance.DescriptionBlock.Text = descModule.Text.Trim();
                }
                else
                {
                    instance.MainContainer.Margin = new Thickness(0, -36, 0, 0);
                    instance.DescriptionBlock.Text = string.Empty;
                }

                if (mainModule != null)
                {
                    if (mainModule.Type == Bilibili.App.Dynamic.V2.ModuleDynamicType.MdlDynPgc)
                    {
                        // instance.CoverImage.ImageUrl = mainModule.DynPgc.Cover + "@500w_350h_1c_100q.jpg";
                    }
                    else if (mainModule.Type == Bilibili.App.Dynamic.V2.ModuleDynamicType.MdlDynArchive)
                    {
                        instance.MainContentPresenter.Content = new VideoViewModel(mainModule.DynArchive);
                        instance.MainContentPresenter.ContentTemplate = instance.VideoTemplate;
                    }
                }

                if (dataModule != null)
                {
                    instance.LikeCountBlock.Text = dataModule.Like.ToString();
                    instance.ReplyCountBlock.Text = dataModule.Reply.ToString();
                }
            }
        }

        private void OnCardClick(object sender, RoutedEventArgs e)
        {
            if (MainContentPresenter.Content is VideoViewModel videoVM)
            {
                AppViewModel.Instance.OpenPlayer(videoVM);
            }
        }
    }
}
