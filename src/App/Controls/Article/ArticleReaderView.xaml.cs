// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Data.Local;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Article;
using Newtonsoft.Json;
using Splat;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Bili.App.Controls.Article
{
    /// <summary>
    /// 阅读器视图.
    /// </summary>
    public partial class ArticleReaderView : CenterPopup
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ArticleItemViewModel), typeof(ArticleReaderView), new PropertyMetadata(null));

        private bool _isPreLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleReaderView"/> class.
        /// </summary>
        protected ArticleReaderView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 实例.
        /// </summary>
        public static ArticleReaderView Instance { get; } = new Lazy<ArticleReaderView>(() => new ArticleReaderView()).Value;

        /// <summary>
        /// 视图模型.
        /// </summary>
        public ArticleItemViewModel ViewModel
        {
            get { return (ArticleItemViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示阅读器.
        /// </summary>
        /// <param name="vm">文章视图模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(ArticleItemViewModel vm)
        {
            Show();
            ViewModel = vm;
            Title = vm.Information.Identifier.Title;
            if (!_isPreLoaded)
            {
                await ReaderWebView.EnsureCoreWebView2Async();
                _isPreLoaded = true;
            }

            vm.ReloadCommand.Execute()
                .Subscribe(async _ =>
                {
                    await LoadContentAsync();
                });
        }

        private async Task LoadContentAsync()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                var detail = await ViewModel.GetDetailAsync();
                var fileToolkit = Locator.Current.GetService<IFileToolkit>();
                var content = detail.Replace("=\"//", "=\"http://")
                    .Replace("data-src", "src");
                var readerContainerStr = await fileToolkit.ReadPackageFile("ms-appx:///Resources/Html/ReaderPage.html");
                var theme = Application.Current.RequestedTheme.ToString();
                var css = await fileToolkit.ReadPackageFile($"ms-appx:///Resources/Html/{theme}.css");

                var html = readerContainerStr.Replace("$theme$", theme.ToLower())
                                            .Replace("$style$", css)
                                            .Replace("$body$", content)
                                            .Replace("$noscroll$", "style=\"-ms-overflow-style: none;\"");
                ReaderWebView.NavigateToString(html);
                ReaderWebView.Focus(FocusState.Pointer);
            });
        }

        private void OnArticleRefreshButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.ReloadCommand.Execute()
                .Subscribe(async _ =>
                {
                    await LoadContentAsync();
                });

        private void OnClosed(object sender, EventArgs e) => ReaderWebView.NavigateToString(string.Empty);

        private async void OnDOMContentLoadedAsync(Windows.UI.Xaml.Controls.WebView sender, Windows.UI.Xaml.Controls.WebViewDOMContentLoadedEventArgs args)
            => await FocusManager.TryFocusAsync(sender, FocusState.Programmatic);

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
