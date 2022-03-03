// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.Storage.Pickers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 下载配置视图模型.
    /// </summary>
    public class DownloadViewModel : ViewModelBase
    {
        private readonly ISettingsToolkit _settingsToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadViewModel"/> class.
        /// </summary>
        protected DownloadViewModel()
        {
            TotalPartCollection = new ObservableCollection<NumberPartViewModel>();
            ServiceLocator.Instance.LoadService(out _settingsToolkit);

            UseMp4Box = ReadSetting(SettingNames.Download_UseMp4Box, false);
            OnlyHevc = ReadSetting(SettingNames.Download_OnlyHevc, false);
            OnlyAvc = ReadSetting(SettingNames.Download_OnlyAvc, false);
            OnlyAudio = ReadSetting(SettingNames.Download_OnlyAudio, false);
            OnlyVideo = ReadSetting(SettingNames.Download_OnlyVideo, false);
            OnlySubtitle = ReadSetting(SettingNames.Download_OnlySubtitle, false);
            UseMultiThread = ReadSetting(SettingNames.Download_UseMultiThread, true);
            UseAppInterface = ReadSetting(SettingNames.Download_UseAppInterface, true);
            UseTvInterface = ReadSetting(SettingNames.Download_UseTvInterface, false);
            UseInternationalInterface = ReadSetting(SettingNames.Download_UseInternationalInterface, false);
            DownloadDanmaku = ReadSetting(SettingNames.Download_DownloadDanmaku, false);
            DownloadFolder = ReadSetting(SettingNames.Download_DownloadFolder, string.Empty);
            UsePartPerfix = ReadSetting(SettingNames.Download_UsePartPerfix, true);
            UseQualitySuffix = ReadSetting(SettingNames.Download_UseQualitySuffix, false);
            UseInteractionQuality = ReadSetting(SettingNames.Download_UseInteractionQuality, false);

            this.WhenAnyValue(x => x.UseMp4Box)
                .Subscribe(x => WriteSetting(SettingNames.Download_UseMp4Box, x));
            this.WhenAnyValue(x => x.OnlyVideo)
                .Subscribe(x => WriteSetting(SettingNames.Download_OnlyVideo, x));
            this.WhenAnyValue(x => x.OnlyAudio)
                .Subscribe(x => WriteSetting(SettingNames.Download_OnlyAudio, x));
            this.WhenAnyValue(x => x.OnlyAvc)
                .Subscribe(x => WriteSetting(SettingNames.Download_OnlyAvc, x));
            this.WhenAnyValue(x => x.OnlyHevc)
                .Subscribe(x => WriteSetting(SettingNames.Download_OnlyHevc, x));
            this.WhenAnyValue(x => x.OnlySubtitle)
                .Subscribe(x => WriteSetting(SettingNames.Download_OnlySubtitle, x));
            this.WhenAnyValue(x => x.UseAppInterface)
                .Subscribe(x => WriteSetting(SettingNames.Download_UseAppInterface, x));
            this.WhenAnyValue(x => x.UseInternationalInterface)
                .Subscribe(x => WriteSetting(SettingNames.Download_UseInternationalInterface, x));
            this.WhenAnyValue(x => x.UseMultiThread)
                .Subscribe(x => WriteSetting(SettingNames.Download_UseMultiThread, x));
            this.WhenAnyValue(x => x.UsePartPerfix)
                .Subscribe(x => WriteSetting(SettingNames.Download_UsePartPerfix, x));
            this.WhenAnyValue(x => x.UseQualitySuffix)
                .Subscribe(x => WriteSetting(SettingNames.Download_UseQualitySuffix, x));
            this.WhenAnyValue(x => x.UseTvInterface)
                .Subscribe(x => WriteSetting(SettingNames.Download_UseTvInterface, x));
            this.WhenAnyValue(x => x.DownloadDanmaku)
                .Subscribe(x => WriteSetting(SettingNames.Download_DownloadDanmaku, x));
            this.WhenAnyValue(x => x.DownloadFolder)
                .Subscribe(x => WriteSetting(SettingNames.Download_DownloadFolder, x));
            this.WhenAnyValue(x => x.UseInteractionQuality)
                .Subscribe(x => WriteSetting(SettingNames.Download_UseInteractionQuality, x));
        }

        /// <summary>
        /// 实例.
        /// </summary>
        public static DownloadViewModel Instance { get; } = new Lazy<DownloadViewModel>(() => new DownloadViewModel()).Value;

        /// <summary>
        /// 下载参数.
        /// </summary>
        public string DownloadParameter { get; set; }

        /// <summary>
        /// 使用MP4Box来混流.
        /// </summary>
        [Reactive]
        public bool UseMp4Box { get; set; }

        /// <summary>
        /// 仅下载HEVC源.
        /// </summary>
        [Reactive]
        public bool OnlyHevc { get; set; }

        /// <summary>
        /// 仅下载AVC源.
        /// </summary>
        [Reactive]
        public bool OnlyAvc { get; set; }

        /// <summary>
        /// 是否仅下载音频.
        /// </summary>
        [Reactive]
        public bool OnlyAudio { get; set; }

        /// <summary>
        /// 是否仅下载视频.
        /// </summary>
        [Reactive]
        public bool OnlyVideo { get; set; }

        /// <summary>
        /// 是否仅下载字幕.
        /// </summary>
        [Reactive]
        public bool OnlySubtitle { get; set; }

        /// <summary>
        /// 使用多线程.
        /// </summary>
        [Reactive]
        public bool UseMultiThread { get; set; }

        /// <summary>
        /// 使用TV接口.
        /// </summary>
        [Reactive]
        public bool UseTvInterface { get; set; }

        /// <summary>
        /// 使用App接口.
        /// </summary>
        [Reactive]
        public bool UseAppInterface { get; set; }

        /// <summary>
        /// 使用国际版接口.
        /// </summary>
        [Reactive]
        public bool UseInternationalInterface { get; set; }

        /// <summary>
        /// 是否下载弹幕.
        /// </summary>
        [Reactive]
        public bool DownloadDanmaku { get; set; }

        /// <summary>
        /// 下载文件夹.
        /// </summary>
        [Reactive]
        public string DownloadFolder { get; set; }

        /// <summary>
        /// 使用分P前缀.
        /// </summary>
        [Reactive]
        public bool UsePartPerfix { get; set; }

        /// <summary>
        /// 使用分P后缀.
        /// </summary>
        [Reactive]
        public bool UseQualitySuffix { get; set; }

        /// <summary>
        /// 使用交互式清晰度选择.
        /// </summary>
        [Reactive]
        public bool UseInteractionQuality { get; set; }

        /// <summary>
        /// 全部分P集合.
        /// </summary>
        public ObservableCollection<NumberPartViewModel> TotalPartCollection { get; }

        /// <summary>
        /// 是否显示分P.
        /// </summary>
        [Reactive]
        public bool IsShowPart { get; set; }

        /// <summary>
        /// 加载.
        /// </summary>
        /// <param name="downloadUrl">下载地址.</param>
        /// <param name="partList">分集列表.</param>
        public void Load(string downloadUrl, List<int> partList)
        {
            DownloadParameter = downloadUrl;
            TotalPartCollection.Clear();
            foreach (var item in partList)
            {
                TotalPartCollection.Add(new NumberPartViewModel(item, true));
            }

            IsShowPart = TotalPartCollection.Count > 1;
        }

        /// <summary>
        /// 设置下载文件夹.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SetDownloadFolderAsync()
        {
            var folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            var folder = await folderPicker.PickSingleFolderAsync().AsTask();
            if (folder != null)
            {
                DownloadFolder = folder.Path;
            }
        }

        /// <summary>
        /// 创建下载命令.
        /// </summary>
        /// <returns>下载命令.</returns>
        public async Task<string> CreateDownloadCommandAsync()
        {
            var list = new List<string>
            {
                "BBDown",
            };

            if (AccountViewModel.Instance.Status == AccountViewModelStatus.Login)
            {
                var authProvider = ServiceLocator.Instance.GetService<IAuthorizeProvider>();
                var token = await authProvider.GetTokenAsync();
                if (UseAppInterface)
                {
                    list.Add("-app");
                }
                else if (UseTvInterface)
                {
                    list.Add("-tv");
                }
                else if (UseInternationalInterface)
                {
                    list.Add("-intl");
                }

                if (UseAppInterface || UseInternationalInterface || UseTvInterface)
                {
                    list.Add("-token");
                    list.Add(token);
                }
            }

            if (UseMp4Box)
            {
                list.Add("--use-mp4box");
            }

            if (OnlyHevc)
            {
                list.Add("-hevc");
            }
            else if (OnlyAvc)
            {
                list.Add("-avc");
            }

            if (UseMultiThread)
            {
                list.Add("-mt");
            }

            if (DownloadDanmaku)
            {
                list.Add("-dd");
            }

            if (!string.IsNullOrEmpty(DownloadFolder))
            {
                list.Add("--work-dir");
                list.Add($"\"{DownloadFolder}\"");
            }

            if (!UsePartPerfix)
            {
                list.Add("--no-part-prefix");
            }

            if (UseQualitySuffix)
            {
                list.Add("--add-dfn-subfix");
            }

            if (UseInteractionQuality)
            {
                list.Add("-ia");
            }

            var selectedPages = TotalPartCollection.Where(p => p.IsSelected).Select(p => p.Data).ToList();
            if (selectedPages.Count > 0)
            {
                var pageStr = string.Join(',', selectedPages);
                list.Add("-p");
                list.Add(pageStr);
            }

            if (OnlyAudio)
            {
                list.Add("--audio-only");
            }
            else if (OnlyVideo)
            {
                list.Add("--video-only");
            }
            else if (OnlySubtitle)
            {
                list.Add("--sub-only");
            }

            list.Add($"\"{DownloadParameter}\"");

            return string.Join(' ', list);
        }

        private void WriteSetting<T>(SettingNames name, T value)
            => _settingsToolkit.WriteLocalSetting(name, value);

        private T ReadSetting<T>(SettingNames name, T defaultValue)
            => _settingsToolkit.ReadLocalSetting(name, defaultValue);
    }
}
