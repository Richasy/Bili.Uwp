// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 互动视频选项面板.
    /// </summary>
    public sealed partial class InteractionChoicePanel : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionChoicePanel"/> class.
        /// </summary>
        public InteractionChoicePanel()
        {
            InitializeComponent();
        }

        private async void OnChoiceClickAsync(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext as InteractionChoice;
            ViewModel.ChangeChoice(data);
            await ViewModel.InitializeInteractionVideoAsync();
        }
    }
}
