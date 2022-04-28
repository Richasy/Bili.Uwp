// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Enums.App;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Overlay
{
    /// <summary>
    /// 消息页面.
    /// </summary>
    public sealed partial class MessagePage : AppPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(MessageModuleViewModel), typeof(MessagePage), new PropertyMetadata(MessageModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePage"/> class.
        /// </summary>
        public MessagePage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public MessageModuleViewModel ViewModel
        {
            get { return (MessageModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckCurrentTypeAsync();
        }

        private void OnNavItemInvokedAsync(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItemContainer as Microsoft.UI.Xaml.Controls.NavigationViewItem;
            MessageType type = default;
            if (item == LikeMeNavItem)
            {
                type = MessageType.Like;
            }
            else if (item == AtMeNavItem)
            {
                type = MessageType.At;
            }
            else
            {
                type = MessageType.Reply;
            }

            ViewModel.CurrentType = type;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentType))
            {
                CheckCurrentTypeAsync();
            }
        }

        private async void CheckCurrentTypeAsync()
        {
            LikeMessageView.Visibility = Visibility.Collapsed;
            AtMessageView.Visibility = Visibility.Collapsed;
            ReplyMessageView.Visibility = Visibility.Collapsed;

            switch (ViewModel.CurrentType)
            {
                case MessageType.Like:
                    LikeMessageView.Visibility = Visibility.Visible;
                    Nav.SelectedItem = LikeMeNavItem;
                    break;
                case MessageType.At:
                    AtMessageView.Visibility = Visibility.Visible;
                    Nav.SelectedItem = AtMeNavItem;
                    break;
                case MessageType.Reply:
                    ReplyMessageView.Visibility = Visibility.Visible;
                    Nav.SelectedItem = ReplyMeNavItem;
                    break;
                default:
                    break;
            }

            if (!ViewModel.IsCurrentTypeRequested())
            {
                await ViewModel.InitializeRequestAsync();
                await AccountViewModel.Instance.InitUnreadAsync();
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }
    }
}
