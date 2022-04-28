// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Bili.App.Controls.Player.Related
{
    /// <summary>
    /// 视频分集.
    /// </summary>
    public sealed partial class VideoPartView : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartView"/> class.
        /// </summary>
        public VideoPartView()
        {
            InitializeComponent();
        }

        private async void OnPartItemClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as VideoPartViewModel;
            if (!data.Data.Equals(ViewModel.CurrentVideoPart))
            {
                await ViewModel.ChangeVideoPartAsync(data.Data.Page.Cid);
            }
            else
            {
                data.IsSelected = true;
            }
        }

        private void RelocateSelectedItem()
        {
            var vm = ViewModel.VideoPartCollection.FirstOrDefault(p => p.IsSelected);
            if (vm != null)
            {
                var index = ViewModel.VideoPartCollection.IndexOf(vm);
                if (index >= 0)
                {
                    PartRepeater.ScrollToItem(index);
                    if (ViewModel.IsOnlyShowIndex)
                    {
                        var ele = IndexRepeater.GetOrCreateElement(index);
                        if (ele != null)
                        {
                            ele.StartBringIntoView(new BringIntoViewOptions { VerticalAlignmentRatio = 0f });
                        }
                    }
                }
            }
        }

        private void OnPartRepeaterLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => RelocateSelectedItem();
    }
}
