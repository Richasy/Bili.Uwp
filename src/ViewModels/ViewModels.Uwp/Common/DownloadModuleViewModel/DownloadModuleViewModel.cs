// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Uwp.Account;
using ReactiveUI;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 下载配置视图模型.
    /// </summary>
    public sealed partial class DownloadModuleViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadModuleViewModel"/> class.
        /// </summary>
        public DownloadModuleViewModel(
            ISettingsToolkit settingsToolkit,
            IResourceToolkit resourceToolkit,
            IAuthorizeProvider authorizeProvider,
            ICallerViewModel callerViewModel,
            AccountViewModel accountViewModel)
        {
            _settingsToolkit = settingsToolkit;
            _resourceToolkit = resourceToolkit;
            _callerViewModel = callerViewModel;
            _accountViewModel = accountViewModel;
            _authorizeProvider = authorizeProvider;
            TotalPartCollection = new ObservableCollection<NumberPartViewModel>();

            ChangeSaveLocationCommand = ReactiveCommand.CreateFromTask(ChangeSaveLocationAsync);
            SaveDownloadTextCommand = ReactiveCommand.CreateFromTask(SaveDownloadCommandAsync);

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
        /// 加载.
        /// </summary>
        /// <param name="downloadParam">下载参数标识，比如视频 Id.</param>
        /// <param name="partList">分集列表.</param>
        public void SetData(string downloadParam, IEnumerable<int> partList)
        {
            DownloadParameter = downloadParam;
            TryClear(TotalPartCollection);
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
        public async Task ChangeSaveLocationAsync()
        {
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.VideosLibrary,
            };

            folderPicker.FileTypeFilter.Add("*");

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
        public async Task SaveDownloadCommandAsync()
        {
            if (TotalPartCollection.Where(p => p.IsSelected).Count() == 0 && TotalPartCollection.Count > 1)
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.AtLeastChooseOnePart), Models.Enums.App.InfoType.Warning);
                return;
            }

            var list = new List<string>
            {
                "BBDown",
            };

            if (_accountViewModel.State == AuthorizeState.SignedIn)
            {
                var token = await _authorizeProvider.GetTokenAsync();
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
                list.Add("--encoding-priority");
                list.Add("hevc");
            }
            else if (OnlyAvc)
            {
                list.Add("--encoding-priority");
                list.Add("avc");
            }
            else if (OnlyAv1)
            {
                list.Add("--encoding-priority");
                list.Add("av1");
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

            var command = string.Join(' ', list);
            var dp = new DataPackage();
            dp.SetText(command);
            Clipboard.SetContent(dp);

            _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.Copied));
        }

        private void WriteSetting<T>(SettingNames name, T value)
            => _settingsToolkit.WriteLocalSetting(name, value);

        private T ReadSetting<T>(SettingNames name, T defaultValue)
            => _settingsToolkit.ReadLocalSetting(name, defaultValue);
    }
}
