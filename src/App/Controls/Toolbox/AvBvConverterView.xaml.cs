// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Toolbox;
using Splat;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// AV号BV号互转.
    /// </summary>
    public sealed partial class AvBvConverterView : CenterPopup
    {
        private readonly IAvBvConverterViewModel _viewModel = Locator.Current.GetService<IAvBvConverterViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AvBvConverterView"/> class.
        /// </summary>
        public AvBvConverterView() => InitializeComponent();

        private void OnOutputBoxGotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => (sender as TextBox).SelectAll();
    }
}
