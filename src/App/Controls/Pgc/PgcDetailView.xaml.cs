// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.App.Pages;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// PGC详情视图.
    /// </summary>
    public sealed partial class PgcDetailView : UserControl
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

        /// <summary>
        /// 显示.
        /// </summary>
        public void Show()
        {
            Container.IsOpen = true;
            ((Window.Current.Content as Frame).Content as RootPage).ShowOnHolder(this);
        }

        private void OnContainerClosed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            ((Window.Current.Content as Frame).Content as RootPage).ClearHolder();
        }
    }
}
