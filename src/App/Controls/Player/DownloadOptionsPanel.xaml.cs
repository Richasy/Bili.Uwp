// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.ViewModels.Uwp;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 下载选项面板.
    /// </summary>
    public sealed partial class DownloadOptionsPanel : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(DownloadViewModel), typeof(DownloadOptionsPanel), new PropertyMetadata(DownloadViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadOptionsPanel"/> class.
        /// </summary>
        public DownloadOptionsPanel()
        {
            this.InitializeComponent();
            Initialize();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DownloadViewModel ViewModel
        {
            get { return (DownloadViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void Initialize()
        {
            if (ViewModel.OnlyVideo)
            {
                DownloadTypeComboBox.SelectedIndex = 1;
            }
            else if (ViewModel.OnlyAudio)
            {
                DownloadTypeComboBox.SelectedIndex = 2;
            }
            else if (ViewModel.OnlySubtitle)
            {
                DownloadTypeComboBox.SelectedIndex = 3;
            }
            else
            {
                DownloadTypeComboBox.SelectedIndex = 0;
            }

            if (ViewModel.OnlyAvc)
            {
                CodecComboBox.SelectedIndex = 1;
            }
            else if (ViewModel.OnlyHevc)
            {
                CodecComboBox.SelectedIndex = 2;
            }
            else
            {
                CodecComboBox.SelectedIndex = 0;
            }
        }

        private void OnDownloadTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = DownloadTypeComboBox.SelectedItem as ComboBoxItem;
            switch (item.Tag)
            {
                case "Full":
                    ViewModel.OnlyVideo = false;
                    ViewModel.OnlyAudio = false;
                    ViewModel.OnlySubtitle = false;
                    break;
                case "Video":
                    ViewModel.OnlyVideo = true;
                    ViewModel.OnlyAudio = false;
                    ViewModel.OnlySubtitle = false;
                    break;
                case "Audio":
                    ViewModel.OnlyVideo = false;
                    ViewModel.OnlyAudio = true;
                    ViewModel.OnlySubtitle = false;
                    break;
                case "Subtitle":
                    ViewModel.OnlyVideo = false;
                    ViewModel.OnlyAudio = false;
                    ViewModel.OnlySubtitle = true;
                    break;
                default:
                    break;
            }
        }

        private void OnCodecComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = CodecComboBox.SelectedItem as ComboBoxItem;
            switch (item.Tag)
            {
                case "Any":
                    ViewModel.OnlyAvc = false;
                    ViewModel.OnlyHevc = false;
                    break;
                case "H264":
                    ViewModel.OnlyAvc = true;
                    ViewModel.OnlyHevc = false;
                    break;
                case "H265":
                    ViewModel.OnlyAvc = false;
                    ViewModel.OnlyHevc = true;
                    break;
                default:
                    break;
            }
        }

        private async void OnGenerateButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var resourceToolkit = ServiceLocator.Instance.GetService<IResourceToolkit>();
            var appVM = AppViewModel.Instance;
            if (ViewModel.TotalPartCollection.Where(p => p.IsSelected).Count() == 0)
            {
                appVM.ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.AtLeastChooseOnePart), Models.Enums.App.InfoType.Warning);
                return;
            }

            var command = await ViewModel.CreateDownloadCommandAsync();
            var package = new DataPackage();
            package.SetText(command);
            Clipboard.SetContent(package);
            appVM.ShowTip(resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Copied));
        }
    }
}
