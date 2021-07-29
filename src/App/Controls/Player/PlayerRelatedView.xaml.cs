// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 播放器的关联视图.
    /// </summary>
    public sealed partial class PlayerRelatedView : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRelatedView"/> class.
        /// </summary>
        public PlayerRelatedView()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsLoading))
            {
                Nav.SelectedItem = Nav.MenuItems.First();
            }
        }

        private void OnNavSelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            RelatedVideoView.Visibility = Nav.SelectedItem == RelatedViedeosItem ?
                Visibility.Visible : Visibility.Collapsed;
        }
    }
}
