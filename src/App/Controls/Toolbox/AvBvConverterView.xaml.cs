// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// AV号BV号互转.
    /// </summary>
    public sealed partial class AvBvConverterView : CenterPopup
    {
        private readonly AvBvConverterViewModel _viewModel = new AvBvConverterViewModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="AvBvConverterView"/> class.
        /// </summary>
        public AvBvConverterView()
        {
            InitializeComponent();
            _viewModel.Dispatcher = Dispatcher;
        }

        private void OnOutputBoxGotFocus(object sender, Windows.UI.Xaml.RoutedEventArgs e)
            => (sender as TextBox).SelectAll();
    }
}
