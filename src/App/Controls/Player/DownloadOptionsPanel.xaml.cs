// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Player
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
            DependencyProperty.Register(nameof(ViewModel), typeof(DownloadModuleViewModel), typeof(DownloadOptionsPanel), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadOptionsPanel"/> class.
        /// </summary>
        public DownloadOptionsPanel()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DownloadModuleViewModel ViewModel
        {
            get { return (DownloadModuleViewModel)GetValue(ViewModelProperty); }
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
            else if (ViewModel.OnlyAv1)
            {
                CodecComboBox.SelectedIndex = 3;
            }
            else
            {
                CodecComboBox.SelectedIndex = 0;
            }

            if (ViewModel.UseAppInterface)
            {
                InterfaceComboBox.SelectedIndex = 1;
            }
            else if (ViewModel.UseTvInterface)
            {
                InterfaceComboBox.SelectedIndex = 2;
            }
            else if (ViewModel.UseInternationalInterface)
            {
                InterfaceComboBox.SelectedIndex = 3;
            }
            else
            {
                InterfaceComboBox.SelectedIndex = 0;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => Initialize();

        private void OnDownloadTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = DownloadTypeComboBox.SelectedItem as ComboBoxItem;
            switch (item.Tag)
            {
                case "Any":
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
                    ViewModel.OnlyAv1 = false;
                    break;
                case "H264":
                    ViewModel.OnlyAvc = true;
                    ViewModel.OnlyHevc = false;
                    ViewModel.OnlyAv1 = false;
                    break;
                case "H265":
                    ViewModel.OnlyAvc = false;
                    ViewModel.OnlyHevc = true;
                    ViewModel.OnlyAv1 = false;
                    break;
                case "AV1":
                    ViewModel.OnlyAvc = false;
                    ViewModel.OnlyHevc = false;
                    ViewModel.OnlyAv1 = true;
                    break;
                default:
                    break;
            }
        }

        private void OnInterfaceComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = InterfaceComboBox.SelectedItem as ComboBoxItem;
            switch (item.Tag)
            {
                case "None":
                    ViewModel.UseAppInterface = false;
                    ViewModel.UseTvInterface = false;
                    ViewModel.UseInternationalInterface = false;
                    break;
                case "App":
                    ViewModel.UseAppInterface = true;
                    ViewModel.UseTvInterface = false;
                    ViewModel.UseInternationalInterface = false;
                    break;
                case "TV":
                    ViewModel.UseAppInterface = false;
                    ViewModel.UseTvInterface = true;
                    ViewModel.UseInternationalInterface = false;
                    break;
                case "International":
                    ViewModel.UseAppInterface = false;
                    ViewModel.UseTvInterface = false;
                    ViewModel.UseInternationalInterface = true;
                    break;
                default:
                    break;
            }
        }
    }
}
