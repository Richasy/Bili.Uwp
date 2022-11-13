// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Bili.Models.App.Constants;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Core
{
    /// <summary>
    /// 本机播放记录视图模型的接口定义.
    /// </summary>
    public sealed partial class RecordViewModel : ViewModelBase, IRecordViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordViewModel"/> class.
        /// </summary>
        public RecordViewModel(
            IFileToolkit fileToolkit,
            ISettingsToolkit settingsToolkit,
            ICallerViewModel callerViewModel)
        {
            _fileToolkit = fileToolkit;
            _settingsToolkit = settingsToolkit;
            _callerViewModel = callerViewModel;

            PlayRecords = new ObservableCollection<PlayRecord>();
            IsShowPlayRecordButton = false;

            CheckContinuePlayCommand = new RelayCommand(CheckContinuePlay);
            AddLastPlayItemCommand = new AsyncRelayCommand<PlaySnapshot>(AddLastPlayItemAsync);
            DeleteLastPlayItemCommand = new AsyncRelayCommand(DeleteLastPlayItemAsync);
            AddPlayRecordCommand = new RelayCommand<PlayRecord>(AddPlayRecord);
            RemovePlayRecordCommand = new RelayCommand<PlayRecord>(RemovePlayRecord);
            ClearPlayRecordCommand = new RelayCommand(ClearPlayRecords);

            PlayRecords.CollectionChanged += OnPlayRecordsCollectionChanged;
        }

        /// <inheritdoc/>
        public Task<PlaySnapshot> GetLastPlayItemAsync()
            => _fileToolkit.ReadLocalDataAsync<PlaySnapshot>(AppConstants.LastOpenVideoFileName);

        private void AddPlayRecord(PlayRecord record)
        {
            PlayRecords.Remove(record);
            PlayRecords.Insert(0, record);
        }

        private void RemovePlayRecord(PlayRecord record)
            => PlayRecords.Remove(record);

        private void ClearPlayRecords()
            => TryClear(PlayRecords);

        private async Task AddLastPlayItemAsync(PlaySnapshot data)
        {
            await _fileToolkit.WriteLocalDataAsync(AppConstants.LastOpenVideoFileName, data);
            _settingsToolkit.WriteLocalSetting(SettingNames.CanContinuePlay, true);
        }

        /// <summary>
        /// 清除本地的继续播放视图模型.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        private async Task DeleteLastPlayItemAsync()
        {
            await _fileToolkit.DeleteLocalDataAsync(AppConstants.LastOpenVideoFileName);
            _settingsToolkit.WriteLocalSetting(SettingNames.CanContinuePlay, false);
        }

        /// <summary>
        /// 检查是否可以继续播放.
        /// </summary>
        private void CheckContinuePlay()
        {
            var supportCheck = _settingsToolkit.ReadLocalSetting(SettingNames.SupportContinuePlay, true);
            var canPlay = _settingsToolkit.ReadLocalSetting(SettingNames.CanContinuePlay, false);
            if (supportCheck && canPlay)
            {
                _callerViewModel.ShowContinuePlayDialog();
            }
        }

        private void OnPlayRecordsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => IsShowPlayRecordButton = PlayRecords.Count > 0;
    }
}
