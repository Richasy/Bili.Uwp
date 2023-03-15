// Copyright (c) Richasy. All rights reserved.

using System;
using System.Diagnostics;
using Bili.DI.Container;
using Bili.Models.Data.Community;
using Bili.Models.Data.Search;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Search;
using Bili.ViewModels.Interfaces.Workspace;
using Microsoft.UI.Xaml;
using Models.Workspace;
using Windows.System;

namespace Bili.Workspace.Pages
{
    /// <summary>
    /// 首页.
    /// </summary>
    public sealed partial class HomePage : HomePageBase
    {
        private readonly IAccountViewModel _accountViewModel;
        private readonly ISearchBoxViewModel _searchBoxViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomePage"/> class.
        /// </summary>
        public HomePage()
        {
            InitializeComponent();
            _accountViewModel = Locator.Instance.GetService<IAccountViewModel>();
            _searchBoxViewModel = Locator.Instance.GetService<ISearchBoxViewModel>();
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
        {
            if (ViewModel.VideoPartitions.Count == 0)
            {
                ViewModel.InitializeVideoPartitionsCommand.Execute(default);
            }

            if (_searchBoxViewModel.HotSearchCollection.Count == 0)
            {
                _searchBoxViewModel.InitializeCommand.Execute(default);
            }
        }

        private void OnVideoPartitionClick(object sender, RoutedEventArgs e)
        {
            if (PartitionFlyout.IsOpen)
            {
                PartitionFlyout.Hide();
            }

            var ele = sender as FrameworkElement;
            var data = ele.DataContext is IVideoPartitionViewModel vpvm
                ? vpvm.Data
                : ele.DataContext as Partition;
            CoreViewModel.NavigateToPartition(data);
        }

        private async void OnQuickTopicClickAsync(object sender, RoutedEventArgs e)
        {
            var settingsToolkit = Locator.Instance.GetService<ISettingsToolkit>();
            var perferLaunch = settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.LaunchType, LaunchType.Web);
            var data = (sender as FrameworkElement).DataContext as QuickTopic;
            var uri = new Uri(perferLaunch == LaunchType.Bili ? data.BiliUrl : data.WebUrl);
            await Launcher.LaunchUriAsync(uri);
        }

        private void OnHotSearchClick(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext as SearchSuggest;
            _searchBoxViewModel.SelectSuggestCommand.Execute(data);
        }
    }

    /// <summary>
    /// <see cref="HomePage"/>的基类.
    /// </summary>
    public class HomePageBase : PageBase<IHomePageViewModel>
    {
    }
}
