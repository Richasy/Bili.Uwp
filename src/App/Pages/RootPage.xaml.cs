// Copyright (c) GodLeaveMe. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.App.Controls;
using Richasy.Bili.Models.App.Args;
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
            this.ViewModel.RequestShowTip += OnRequestShowTip;
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
        /// <param name="needDisableBackButton">是否需要禁用返回按钮.</param>
        public void ShowOnHolder(UIElement element, bool needDisableBackButton = true)
        {
            if (!HolderContainer.Children.Contains(element))
            {
                HolderContainer.Children.Add(element);
            }

            HolderContainer.Visibility = Visibility.Visible;

            if (needDisableBackButton)
            {
                ViewModel.IsBackButtonEnabled = false;
            }
        }

        /// <summary>
        /// 清除顶层视图.
        /// </summary>
        public void ClearHolder()
        {
            HolderContainer.Children.Clear();
            ViewModel.IsBackButtonEnabled = true;
        }

        /// <summary>
        /// 从顶层视图中移除元素.
        /// </summary>
        /// <param name="element">UI元素.</param>
        public void RemoveFromHolder(UIElement element)
        {
            HolderContainer.Children.Remove(element);
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

        private void OnRequestShowTip(object sender, AppTipNotificationEventArgs e)
        {
            new TipPopup(e.Message).ShowAsync(e.Type);
        }
    }
}
