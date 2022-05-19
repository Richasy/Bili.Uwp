// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Pages.Desktop;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Bili.App.Controls
{
    /// <summary>
    /// 居中显示的浮出层.
    /// </summary>
    public partial class CenterPopup : ContentControl
    {
        private readonly NavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CenterPopup"/> class.
        /// </summary>
        public CenterPopup()
        {
            DefaultStyleKey = typeof(CenterPopup);
            _navigationViewModel = Splat.Locator.Current.GetService<NavigationViewModel>();
        }

        /// <summary>
        /// 显示弹出层.
        /// </summary>
        public void Show()
        {
            RootPage.Current.ShowOnHolder(this);
        }

        /// <summary>
        /// 隐藏弹出层.
        /// </summary>
        public void Hide()
        {
            _navigationViewModel.BackCommand.Execute().Subscribe();
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

        private void OnCloseButtonClick(object sender, RoutedEventArgs e) => Hide();
    }
}
