// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 阅读器视图.
    /// </summary>
    public partial class ReaderView : CenterPopup
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ArticleViewModel), typeof(ReaderView), new PropertyMetadata(null));

        private bool _isPreLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderView"/> class.
        /// </summary>
        protected ReaderView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 实例.
        /// </summary>
        public static ReaderView Instance { get; } = new Lazy<ReaderView>(() => new ReaderView()).Value;

        /// <summary>
        /// 视图模型.
        /// </summary>
        public ArticleViewModel ViewModel
        {
            get { return (ArticleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示阅读器.
        /// </summary>
        /// <param name="vm">文章视图模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(ArticleViewModel vm)
        {
            Show();
            ViewModel = vm;
            Title = vm.Title;
            if (!_isPreLoaded)
            {
                await ReaderWebView.EnsureCoreWebView2Async();
                _isPreLoaded = true;
            }

            if (string.IsNullOrEmpty(vm.ArticleContent))
            {
                await ViewModel.InitializeArticleContentAsync();
                await LoadContentAsync();
            }
            else
            {
                await LoadContentAsync();
            }
        }

        private async Task LoadContentAsync()
        {
            if (ViewModel != null && !string.IsNullOrEmpty(ViewModel.ArticleContent))
            {
                var fileToolkit = ServiceLocator.Instance.GetService<IFileToolkit>();
                var content = ViewModel.ArticleContent.Replace("=\"//", "=\"http://")
                    .Replace("data-src", "src");
                var readerContainerStr = await fileToolkit.ReadPackageFile("ms-appx:///Resources/Html/ReaderPage.html");
                var theme = App.Current.RequestedTheme.ToString();
                var css = await fileToolkit.ReadPackageFile($"ms-appx:///Resources/Html/{theme}.css");

                var html = readerContainerStr.Replace("$theme$", theme.ToLower())
                                            .Replace("$style$", css)
                                            .Replace("$body$", content)
                                            .Replace("$noscroll$", "style=\"-ms-overflow-style: none;\"");
                ReaderWebView.NavigateToString(html);
                ReaderWebView.Focus(FocusState.Pointer);
            }
        }

        private async void OnArticleRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeArticleContentAsync();
            await LoadContentAsync();
        }

        private void OnClosed(object sender, EventArgs e) => ReaderWebView.NavigateToString(string.Empty);

        private async void OnScriptNotifyAsync(object sender, Windows.UI.Xaml.Controls.NotifyEventArgs e)
        {
            var data = JsonConvert.DeserializeObject<KeyValue<string>>(e.Value);
            if (data.Key == AppConstants.LinkClickEvent)
            {
                await Launcher.LaunchUriAsync(new Uri(data.Value));
            }
        }

        private async void OnDOMContentLoadedAsync(Windows.UI.Xaml.Controls.WebView sender, Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs args)
        {
            await FocusManager.TryFocusAsync(sender, FocusState.Programmatic);
        }

        private async void OnWebMessageReceivedAsync(Microsoft.UI.Xaml.Controls.WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs args)
        {
            var data = JsonConvert.DeserializeObject<KeyValue<string>>(args.WebMessageAsJson);
            if (data.Key == AppConstants.LinkClickEvent)
            {
                await Launcher.LaunchUriAsync(new Uri(data.Value));
            }
        }
    }
}
