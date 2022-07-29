// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Pgc;
using Windows.UI.Xaml;

namespace Bili.App.Controls
{
    /// <summary>
    /// PGC播放列表视图.
    /// </summary>
    public sealed partial class PgcPlayListDetailView : CenterPopup
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(IPgcPlaylistViewModel), typeof(PgcPlayListDetailView), new PropertyMetadata(default, new PropertyChangedCallback(OnViewModelChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPlayListDetailView"/> class.
        /// </summary>
        public PgcPlayListDetailView() => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public IPgcPlaylistViewModel ViewModel
        {
            get { return (IPgcPlaylistViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as PgcPlayListDetailView;
            instance.DataContext = e.NewValue;
        }
    }
}
