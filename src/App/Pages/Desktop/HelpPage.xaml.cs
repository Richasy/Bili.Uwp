// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Uwp;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class HelpPage : AppPage
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
            InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public HelpViewModel ViewModel
        {
            get { return (HelpViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnAskIssueButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/Richasy/Bili.Uwp/issues/new/choose")).AsTask();
        }

        private async void OnProjectHomePageClickAsync(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://github.com/Richasy/Bili.Uwp/")).AsTask();
        }

        private async void OnBiliHomePageClickAsync(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("https://space.bilibili.com/5992670")).AsTask();
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

        private async void OnLinkViewItemClickAsync(object sender, ItemClickEventArgs e)
        {
            var data = e.ClickedItem as Models.App.Other.KeyValue<string>;
            await Launcher.LaunchUriAsync(new Uri(data.Value));
        }
    }
}
