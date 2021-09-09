// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// PGC详情视图.
    /// </summary>
    public sealed partial class PgcDetailView : CenterPopup
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PlayerViewModel), typeof(PgcDetailView), new PropertyMetadata(PlayerViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcDetailView"/> class.
        /// </summary>
        public PgcDetailView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PlayerViewModel ViewModel
        {
            get { return (PlayerViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}
