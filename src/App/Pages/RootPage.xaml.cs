// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.App.Controls;
using Richasy.Bili.App.Controls.Dialogs;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// The page is used for default loading.
    /// </summary>
    public sealed partial class RootPage : AppPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootPage"/> class.
        /// </summary>
        public RootPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
            this.CoreViewModel.RequestShowTip += OnRequestShowTip;
            this.CoreViewModel.RequestBack += OnRequestBackAsync;
            this.CoreViewModel.RequestShowUpdateDialog += OnRequestShowUpdateDialogAsync;
            CoreViewModel.RequestContinuePlay += OnRequestContinuePlayAsync;
            SizeChanged += OnSizeChanged;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequestedAsync;
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
                CoreViewModel.IsBackButtonEnabled = false;
            }
        }

        /// <summary>
        /// 清除顶层视图.
        /// </summary>
        public void ClearHolder()
        {
            HolderContainer.Children.Clear();
            CoreViewModel.IsBackButtonEnabled = true;
        }

        /// <summary>
        /// 从顶层视图中移除元素.
        /// </summary>
        /// <param name="element">UI元素.</param>
        public void RemoveFromHolder(UIElement element)
        {
            HolderContainer.Children.Remove(element);
            if (HolderContainer.Children.Count == 0)
            {
                CoreViewModel.IsBackButtonEnabled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == Windows.UI.Input.PointerUpdateKind.XButton1Released)
            {
                e.Handled = true;
                CoreViewModel.Back();
            }

            base.OnPointerReleased(e);
        }

        private async Task<bool> TryBackAsync()
        {
            if (HolderContainer.Children.Count > 0)
            {
                HolderContainer.Children.Remove(HolderContainer.Children.Last());
                return true;
            }
            else if (BiliPlayerTransportControls.Instance != null && BiliPlayerTransportControls.Instance.CheckBack())
            {
                return true;
            }
            else if (CoreViewModel.IsBackButtonEnabled)
            {
                return await TitleBar.TryBackAsync();
            }

            return false;
        }

        private async void OnRequestBackAsync(object sender, System.EventArgs e) => await TryBackAsync();

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            this.CoreViewModel.PropertyChanged += OnViewModelPropertyChanged;
            this.CoreViewModel.RequestPlay += OnRequestPlay;
            CoreViewModel.InitializePadding();
            CoreViewModel.CheckContinuePlay();
            await AccountViewModel.Instance.TrySignInAsync(true);
#if !DEBUG
            await CoreViewModel.CheckUpdateAsync();
#endif
        }

        private async void OnBackRequestedAsync(object sender, BackRequestedEventArgs e)
        {
            if (await TryBackAsync())
            {
                e.Handled = true;
            }
        }

        private void OnRequestPlay(object sender, object e)
            => OverFrame.Navigate(typeof(Overlay.PlayerPage), e, new DrillInNavigationTransitionInfo());

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
            => CoreViewModel.InitializePadding();

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CoreViewModel.IsOpenPlayer))
            {
                if (!CoreViewModel.IsOpenPlayer)
                {
                    CoreViewModel.ReleaseDisplayRequest();
                    OverFrame.Navigate(typeof(Page));
                }
                else
                {
                    CoreViewModel.ActiveDisplayRequest();
                }
            }
            else if (e.PropertyName == nameof(CoreViewModel.IsOverLayerExtendToTitleBar))
            {
                var stateName = CoreViewModel.IsOverLayerExtendToTitleBar ? nameof(ExtendedOverState) : nameof(DefaultOverState);
                VisualStateManager.GoToState(this, stateName, false);
            }
        }

        private void OnRequestShowTip(object sender, AppTipNotificationEventArgs e)
            => new TipPopup(e.Message).ShowAsync(e.Type);

        private async void OnRequestShowUpdateDialogAsync(object sender, UpdateEventArgs e)
            => await new UpgradeDialog(e).ShowAsync();

        private async void OnRequestContinuePlayAsync(object sender, EventArgs e)
            => await new ContinuePlayDialog().ShowAsync();
    }
}
