// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Richasy.Bili.App.Pages;
using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// Root navigation view.
    /// </summary>
    public sealed partial class RootNavigationView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AppViewModel), typeof(RootNavigationView), new PropertyMetadata(AppViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="RootNavigationView"/> class.
        /// </summary>
        public RootNavigationView()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public AppViewModel ViewModel
        {
            get { return (AppViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged -= this.OnAppViewModelPropertyChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += this.OnAppViewModelPropertyChanged;
            CheckMainContentNavigation();
        }

        private void OnAppViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentMainContentId))
            {
                CheckMainContentNavigation();
            }
        }

        private void OnRootNavViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                ViewModel.SetMainContentId(Models.Enums.PageIds.Settings);
            }
            else
            {
                var pageId = NavigationExtension.GetPageId(args.InvokedItemContainer);
                ViewModel.SetMainContentId(pageId);
            }
        }

        private void CheckMainContentNavigation()
        {
            var pageId = ViewModel.CurrentMainContentId;
            Type pageType = null;
            switch (pageId)
            {
                case Models.Enums.PageIds.None:
                default:
                    break;
                case Models.Enums.PageIds.Home:
                    pageType = typeof(HomePage);
                    break;
                case Models.Enums.PageIds.Rank:
                    pageType = typeof(RankPage);
                    break;
                case Models.Enums.PageIds.Partition:
                    pageType = typeof(PartitionPage);
                    break;
                case Models.Enums.PageIds.Channel:
                    pageType = typeof(ChannelPage);
                    break;
                case Models.Enums.PageIds.SpecialColumn:
                    pageType = typeof(SpecialColumnPage);
                    break;
                case Models.Enums.PageIds.Live:
                    pageType = typeof(LivePage);
                    break;
                case Models.Enums.PageIds.DynamicFeed:
                    pageType = typeof(DynamicFeedPage);
                    break;
                case Models.Enums.PageIds.MyFavorite:
                    pageType = typeof(MyFavoritePage);
                    break;
                case Models.Enums.PageIds.SeeLater:
                    pageType = typeof(SeeLaterPage);
                    break;
                case Models.Enums.PageIds.ViewHistory:
                    pageType = typeof(ViewHistoryPage);
                    break;
                case Models.Enums.PageIds.Help:
                    pageType = typeof(HelpPage);
                    break;
                case Models.Enums.PageIds.Settings:
                    pageType = typeof(SettingPage);
                    break;
            }

            if (pageType != null)
            {
                MainFrame.Navigate(pageType, null, new DrillInNavigationTransitionInfo());
            }
        }
    }
}
