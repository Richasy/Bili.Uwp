// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class HelpPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(HelpViewModel), typeof(HelpPage), new PropertyMetadata(HelpViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpPage"/> class.
        /// </summary>
        public HelpPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public HelpViewModel ViewModel
        {
            get { return (HelpViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnAskIssueButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/GodLeaveMe/BilibiliUWP2/issues/new")).AsTask();
        }

        private async void OnProjectHomePageClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/GodLeaveMe/BilibiliUWP2/releases")).AsTask();
        }

        private async void OnBiliHomePageClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://space.bilibili.com/166740145")).AsTask();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = Window.Current.Bounds.Width;
            if (width < AppViewModel.Instance.MediumWindowThresholdWidth)
            {
                if (WideContainer.Children.Count > 0)
                {
                    WideContainer.Children.Clear();
                    NarrowContainer.Children.Add(AboutContainer);
                    NarrowContainer.Children.Add(QuestionContainer);
                    Grid.SetColumn(QuestionContainer, 0);
                    Grid.SetRow(QuestionContainer, 1);
                }
            }
            else
            {
                if (NarrowContainer.Children.Count > 0)
                {
                    NarrowContainer.Children.Clear();
                    WideContainer.Children.Add(AboutContainer);
                    WideContainer.Children.Add(QuestionContainer);
                    Grid.SetColumn(QuestionContainer, 1);
                    Grid.SetRow(QuestionContainer, 0);
                }
            }
        }
    }
}
