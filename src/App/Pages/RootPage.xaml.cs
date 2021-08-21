// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// The page is used for default loading.
    /// </summary>
    public sealed partial class RootPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AppViewModel), typeof(RootPage), new PropertyMetadata(AppViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="RootPage"/> class.
        /// </summary>
        public RootPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 应用视图模型.
        /// </summary>
        public AppViewModel ViewModel
        {
            get { return (AppViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示顶层视图.
        /// </summary>
        /// <param name="element">要显示的元素.</param>
        public void ShowOnHolder(UIElement element)
        {
            if (!HolderContainer.Children.Contains(element))
            {
                HolderContainer.Children.Add(element);
            }

            HolderContainer.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 清除顶层视图.
        /// </summary>
        public void ClearHolder()
        {
            HolderContainer.Children.Clear();
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            this.ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            this.ViewModel.RequestPlay += OnRequestPlay;
            await AccountViewModel.Instance.TrySignInAsync(true);
        }

        private void OnRequestPlay(object sender, object e)
        {
            OverFrame.Navigate(typeof(Overlay.PlayerPage), e, new DrillInNavigationTransitionInfo());
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsOpenPlayer))
            {
                if (!ViewModel.IsOpenPlayer)
                {
                    ViewModel.ReleaseDisplayRequest();
                    OverFrame.Navigate(typeof(Page));
                }
                else
                {
                    ViewModel.ActiveDisplayRequest();
                }
            }
            else if (e.PropertyName == nameof(ViewModel.IsOverLayerExtendToTitleBar))
            {
                var stateName = ViewModel.IsOverLayerExtendToTitleBar ? nameof(ExtendedOverState) : nameof(DefaultOverState);
                VisualStateManager.GoToState(this, stateName, false);
            }
        }
    }
}
