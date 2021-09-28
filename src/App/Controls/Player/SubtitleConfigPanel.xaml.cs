// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 字幕配置面板.
    /// </summary>
    public sealed partial class SubtitleConfigPanel : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtitleConfigPanel"/> class.
        /// </summary>
        public SubtitleConfigPanel()
        {
            this.InitializeComponent();
        }

        private async void OnSubtitleIndexClickAsync(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext as SubtitleIndexItemViewModel;
            if (data.IsSelected)
            {
                data.IsSelected = false;
                data.IsSelected = true;
                ViewModel.CheckSubtitleSelection();
            }
            else
            {
                ViewModel.CurrentSubtitleIndex = data.Data;
                ViewModel.CheckSubtitleSelection();
                await ViewModel.InitializeSubtitleAsync(data.Data);
            }
        }
    }
}
