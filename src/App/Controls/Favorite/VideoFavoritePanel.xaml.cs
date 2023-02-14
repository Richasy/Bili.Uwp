// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.App.Controls.Dialogs;
using Bili.DI.Container;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
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
            ViewModel = Locator.Instance.GetService<IVideoFavoriteModuleViewModel>();
            DataContext = ViewModel;
        }

        /// <summary>
        /// 核心视图模型.
        /// </summary>
        public IAppViewModel CoreViewModel { get; } = Locator.Instance.GetService<IAppViewModel>();

        private async void OnRemoveFavoriteButtonClickAsync(object sender, RoutedEventArgs e)
            => await RemoveAsync(sender);

        private async Task RemoveAsync(object sender)
        {
            var vm = (sender as FrameworkElement).DataContext as IVideoFavoriteFolderViewModel;
            var resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            var warning = vm.IsMine
                ? resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.DeleteFavoriteWarning)
                : resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.UnFavoriteWarning);
            var dialog = new ConfirmDialog(warning);
            var isConfirm = (await dialog.ShowAsync()) == ContentDialogResult.Primary;

            if (isConfirm)
            {
                _ = vm.RemoveCommand.ExecuteAsync(null);
            }
        }

        private void OnDefaultExpanderClick(object sender, RoutedEventArgs e)
            => ViewModel.ShowDefaultFolderDetailCommand.Execute(null);
    }

    /// <summary>
    /// <see cref="VideoFavoritePanel"/> 的基类.
    /// </summary>
    public class VideoFavoritePanelBase : ReactiveUserControl<IVideoFavoriteModuleViewModel>
    {
    }
}
