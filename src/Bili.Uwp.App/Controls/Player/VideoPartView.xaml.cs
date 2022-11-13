// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.ViewModels.Interfaces.Video;
using Windows.UI.Xaml;

namespace Bili.Uwp.App.Controls.Player
{
    /// <summary>
    /// 视频分集.
    /// </summary>
    public sealed partial class VideoPartView : VideoPartViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartView"/> class.
        /// </summary>
        public VideoPartView() => InitializeComponent();

        private void OnPartItemClick(object sender, RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as IVideoIdentifierSelectableViewModel;
            if (!data.Data.Equals(ViewModel.CurrentVideoPart))
            {
                ViewModel.ChangeVideoPartCommand.Execute(data.Data);
            }
            else
            {
                data.IsSelected = true;
            }
        }

        private void RelocateSelectedItem()
        {
            var vm = ViewModel.VideoParts.FirstOrDefault(p => p.IsSelected);
            if (vm != null)
            {
                var index = ViewModel.VideoParts.IndexOf(vm);
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

        private void OnPartRepeaterLoaded(object sender, RoutedEventArgs e)
            => RelocateSelectedItem();
    }

    /// <summary>
    /// <see cref="VideoPartView"/> 的基类.
    /// </summary>
    public class VideoPartViewBase : ReactiveUserControl<IVideoPlayerPageViewModel>
    {
    }
}
