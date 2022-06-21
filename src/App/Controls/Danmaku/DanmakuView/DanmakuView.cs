﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Atelier39;
using Bili.ViewModels.Uwp.Common;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Danmaku
{
    /// <summary>
    /// 弹幕控件.
    /// </summary>
    public sealed partial class DanmakuView : ReactiveControl<DanmakuModuleViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Danmaku"/> class.
        /// </summary>
        public DanmakuView()
        {
            DefaultStyleKey = typeof(DanmakuView);
            Loaded += OnLoadedAsync;
            Unloaded += OnUnloaded;
        }

        internal override async void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is DanmakuModuleViewModel viewModel)
            {
                viewModel.PropertyChanged += OnViewModelProeprtyChanged;
                await RedrawAsync();
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _rootGrid = GetTemplateChild(RootGridName) as Grid;
        }

        private DanmakuFontSize GetFontSize(double fontSize)
        {
            return fontSize switch
            {
                0.5 => DanmakuFontSize.Smallest,
                1 => DanmakuFontSize.Smaller,
                2.0 => DanmakuFontSize.Larger,
                2.5 => DanmakuFontSize.Largest,
                _ => DanmakuFontSize.Normal,
            };
        }

        private void InitializeController()
        {
            _rootGrid.Children.Clear();
            _canvas = new CanvasAnimatedControl();
            _rootGrid.Children.Add(_canvas);

            if (_canvas != null && ViewModel != null)
            {
                _danmakuController = new DanmakuFrostMaster(_canvas);
                _danmakuController.SetAutoControlDensity(ViewModel.IsDanmakuLimit);
                _danmakuController.SetRollingDensity(-1);
                _danmakuController.SetBorderColor(Colors.Gray);
                _danmakuController.SetRollingAreaRatio(Convert.ToInt32(ViewModel.DanmakuArea * 10));
                _danmakuController.SetDanmakuFontSizeOffset(GetFontSize(ViewModel.DanmakuFontSize));
                _danmakuController.SetFontFamilyName(ViewModel.DanmakuFont ?? "Segoe UI");
                _danmakuController.SetRollingSpeed(Convert.ToInt32(ViewModel.DanmakuSpeed * 5));
                _danmakuController.SetIsTextBold(ViewModel.IsDanmakuBold);
                _isInitialized = true;
            }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!_isInitialized)
            {
                await RedrawAsync();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Close();
            ViewModel.PropertyChanged -= OnViewModelProeprtyChanged;
        }

        private void OnViewModelProeprtyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.DanmakuSpeed))
            {
                var speed = (double)ViewModel.DanmakuSpeed * 5;
                _danmakuController?.SetRollingSpeed(Convert.ToInt32(speed));
            }
            else if (e.PropertyName == nameof(ViewModel.DanmakuArea))
            {
                var value = ViewModel.DanmakuArea;
                if (value <= 0)
                {
                    value = 0.1;
                }
                else if (value > 1)
                {
                    value = 1;
                }

                _danmakuController?.SetRollingAreaRatio(Convert.ToInt32(value * 10));
            }
            else if (e.PropertyName == nameof(ViewModel.DanmakuFontSize))
            {
                _danmakuController?.SetDanmakuFontSizeOffset(GetFontSize(ViewModel.DanmakuFontSize));
            }
            else if (e.PropertyName == nameof(ViewModel.DanmakuFont))
            {
                _danmakuController?.SetFontFamilyName(ViewModel.DanmakuFont?.ToString() ?? "Segoe UI");
            }
            else if (e.PropertyName == nameof(ViewModel.IsDanmakuBold))
            {
                _danmakuController?.SetIsTextBold(ViewModel.IsDanmakuBold);
            }
            else if (e.PropertyName == nameof(ViewModel.IsDanmakuLimit))
            {
                _danmakuController?.SetAutoControlDensity(ViewModel.IsDanmakuLimit);
            }
        }
    }
}
