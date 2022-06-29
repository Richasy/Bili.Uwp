// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.App.Controls.Dialogs;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Favorite
{
    /// <summary>
    /// 视频收藏夹面板.
    /// </summary>
    public sealed partial class VideoFavoritePanel : VideoFavoritePanelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoritePanel"/> class.
        /// </summary>
        public VideoFavoritePanel()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<VideoFavoriteModuleViewModel>();
            DataContext = ViewModel;
        }

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public AppViewModel CoreViewModel { get; } = Locator.Current.GetService<AppViewModel>();

        private async void OnRemoveFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
            => await RemoveAsync(sender);

        private async Task RemoveAsync(object sender)
        {
            var vm = (sender as FrameworkElement).DataContext as VideoFavoriteFolderViewModel;
            var resourceToolkit = Locator.Current.GetService<IResourceToolkit>();
            var warning = vm.IsMine
                ? resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.DeleteFavoriteWarning)
                : resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.UnFavoriteWarning);
            var dialog = new ConfirmDialog(warning);
            var isConfirm = (await dialog.ShowAsync()) == ContentDialogResult.Primary;

            if (isConfirm)
            {
                vm.RemoveCommand.Execute().Subscribe();
            }
        }

        private void OnDefaultExpanderClick(object sender, Richasy.ExpanderEx.Uwp.ExpanderExClickEventArgs e)
            => ViewModel.ShowDefaultFolderDetailCommand.Execute().Subscribe();
    }

    /// <summary>
    /// <see cref="VideoFavoritePanel"/> 的基类.
    /// </summary>
    public class VideoFavoritePanelBase : ReactiveUserControl<VideoFavoriteModuleViewModel>
    {
    }
}
