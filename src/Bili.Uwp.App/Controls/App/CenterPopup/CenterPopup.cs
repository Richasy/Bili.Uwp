// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Uwp.App.Pages.Desktop;
using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Bili.Uwp.App.Controls
{
    /// <summary>
    /// 居中显示的浮出层.
    /// </summary>
    public partial class CenterPopup : ContentControl
    {
        private readonly INavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CenterPopup"/> class.
        /// </summary>
        public CenterPopup()
        {
            DefaultStyleKey = typeof(CenterPopup);
            _navigationViewModel = Locator.Instance.GetService<INavigationViewModel>();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 显示弹出层.
        /// </summary>
        public void Show()
            => RootPage.Current.ShowOnHolder(this);

        /// <summary>
        /// 隐藏弹出层.
        /// </summary>
        public void Hide()
        {
            _navigationViewModel.BackCommand.Execute(null);
            Closed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.PointerUpdateKind == Windows.UI.Input.PointerUpdateKind.XButton1Released)
            {
                e.Handled = true;
                Hide();
            }

            base.OnPointerReleased(e);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _closeButton = GetTemplateChild(CloseButtonName) as Button;
            if (_closeButton != null)
            {
                _closeButton.Click += OnCloseButtonClick;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
            => Focus(FocusState.Programmatic);

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
            => Hide();
    }
}
