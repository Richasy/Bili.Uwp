// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 下载配置视图模型.
    /// </summary>
    public sealed partial class DownloadModuleViewModel : ViewModelBase, IDownloadModuleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadModuleViewModel"/> class.
        /// </summary>
        public DownloadModuleViewModel(
            ISettingsToolkit settingsToolkit,
            IResourceToolkit resourceToolkit,
            IAuthorizeProvider authorizeProvider,
            ICallerViewModel callerViewModel,
            IAccountViewModel accountViewModel)
        {
            _settingsToolkit = settingsToolkit;
            _resourceToolkit = resourceToolkit;
            _callerViewModel = callerViewModel;
            _accountViewModel = accountViewModel;
            _authorizeProvider = authorizeProvider;
            TotalPartCollection = new ObservableCollection<INumberPartViewModel>();

            ChangeSaveLocationCommand = new AsyncRelayCommand(ChangeSaveLocationAsync);
            SaveDownloadTextCommand = new AsyncRelayCommand(SaveDownloadCommandAsync);

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
        }

        /// <inheritdoc/>
        public void SetData(string downloadParam, IEnumerable<int> partList)
        {
            DownloadParameter = downloadParam;
            TryClear(TotalPartCollection);
            foreach (var item in partList)
            {
                var vm = Locator.Instance.GetService<INumberPartViewModel>();
                vm.InjectData(item);
                vm.IsSelected = true;
                TotalPartCollection.Add(vm);
            }

            IsShowPart = TotalPartCollection.Count > 1;
        }

        /// <summary>
        /// 设置下载文件夹.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task ChangeSaveLocationAsync()
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
        private async Task SaveDownloadCommandAsync()
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

        partial void OnUseMp4BoxChanged(bool value)
         => WriteSetting(SettingNames.Download_UseMp4Box, value);

        partial void OnOnlyVideoChanged(bool value)
            => WriteSetting(SettingNames.Download_OnlyVideo, value);

        partial void OnOnlyAudioChanged(bool value)
            => WriteSetting(SettingNames.Download_OnlyAudio, value);

        partial void OnOnlyAvcChanged(bool value)
            => WriteSetting(SettingNames.Download_OnlyAvc, value);

        partial void OnOnlyHevcChanged(bool value)
            => WriteSetting(SettingNames.Download_OnlyHevc, value);

        partial void OnOnlySubtitleChanged(bool value)
            => WriteSetting(SettingNames.Download_OnlySubtitle, value);

        partial void OnUseAppInterfaceChanged(bool value)
            => WriteSetting(SettingNames.Download_UseAppInterface, value);

        partial void OnUseInternationalInterfaceChanged(bool value)
            => WriteSetting(SettingNames.Download_UseInternationalInterface, value);

        partial void OnUseMultiThreadChanged(bool value)
            => WriteSetting(SettingNames.Download_UseMultiThread, value);

        partial void OnUseTvInterfaceChanged(bool value)
            => WriteSetting(SettingNames.Download_UseTvInterface, value);

        partial void OnDownloadDanmakuChanged(bool value)
            => WriteSetting(SettingNames.Download_DownloadDanmaku, value);

        partial void OnDownloadFolderChanged(string value)
            => WriteSetting(SettingNames.Download_DownloadFolder, value);

        partial void OnUseInteractionQualityChanged(bool value)
            => WriteSetting(SettingNames.Download_UseInteractionQuality, value);
    }
}
