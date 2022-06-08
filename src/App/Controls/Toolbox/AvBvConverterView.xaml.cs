// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Toolbox;
using Splat;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// AV号BV号互转.
    /// </summary>
    public sealed partial class AvBvConverterView : CenterPopup
    {
        private readonly AvBvConverterViewModel _viewModel = Splat.Locator.Current.GetService<AvBvConverterViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AvBvConverterView"/> class.
        /// </summary>
        public AvBvConverterView() => InitializeComponent();

        private void OnOutputBoxGotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => (sender as TextBox).SelectAll();
    }
}
