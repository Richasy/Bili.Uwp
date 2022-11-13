// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Common
{
    /// <summary>
    /// 字幕模块视图模型.
    /// </summary>
    public sealed partial class SubtitleModuleViewModel : ViewModelBase, ISubtitleModuleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtitleModuleViewModel"/> class.
        /// </summary>
        public SubtitleModuleViewModel(
            IPlayerProvider playerProvider,
            ISettingsToolkit settingsToolkit)
        {
            _playerProvider = playerProvider;
            _settingsToolkit = settingsToolkit;
            _subtitles = new List<SubtitleInformation>();
            Metas = new ObservableCollection<SubtitleMeta>();
            ConvertTypeCollection = new ObservableCollection<SubtitleConvertType>
            {
                SubtitleConvertType.None,
                SubtitleConvertType.ToTraditionalChinese,
                SubtitleConvertType.ToSimplifiedChinese,
            };

            ConvertType = _settingsToolkit.ReadLocalSetting(SettingNames.SubtitleConvertType, SubtitleConvertType.None);
            CanShowSubtitle = _settingsToolkit.ReadLocalSetting(SettingNames.CanShowSubtitle, true);

            ReloadCommand = new AsyncRelayCommand(ReloadAsync);
            ChangeMetaCommand = new AsyncRelayCommand<SubtitleMeta>(ChangeMetaAsync);
            SeekCommand = new RelayCommand<double>(Seek);

            AttachIsRunningToAsyncCommand(p => IsReloading = p, ReloadCommand);
            AttachExceptionHandlerToAsyncCommand(LogException, ReloadCommand);
        }

        /// <inheritdoc/>
        public void SetData(string mainId, string partId)
        {
            _mainId = mainId;
            _partId = partId;
            ReloadCommand.ExecuteAsync(null);
        }

        private void Reset()
        {
            _subtitles.Clear();
            TryClear(Metas);
            CurrentMeta = null;
            HasSubtitles = false;
            CurrentSubtitle = string.Empty;
        }

        private async Task ReloadAsync()
        {
            Reset();
            var data = await _playerProvider.GetSubtitleIndexAsync(_mainId, _partId);
            if (data != null
                && data.Count() > 0)
            {
                HasSubtitles = true;
                data.ToList().ForEach(p => Metas.Add(p));
                await ChangeMetaAsync(Metas.First());
            }
        }

        private async Task ChangeMetaAsync(SubtitleMeta meta)
        {
            CurrentMeta = meta;
            _subtitles.Clear();
            CurrentSubtitle = string.Empty;
            var subtitles = await _playerProvider.GetSubtitleDetailAsync(meta.Url);
            foreach (var subtitle in subtitles)
            {
                _subtitles.Add(subtitle);
            }

            SeekCommand.Execute(_currentSeconds);
        }

        private void Seek(double sec)
        {
            _currentSeconds = sec;
            if (!HasSubtitles || CurrentMeta == null)
            {
                return;
            }

            var subtitle = _subtitles.FirstOrDefault(p => p.StartPosition <= sec && p.EndPosition >= sec);
            CurrentSubtitle = subtitle != null && !string.IsNullOrEmpty(subtitle.Content)
                ? ConvertType switch
                {
                    SubtitleConvertType.ToSimplifiedChinese => ToolGood.Words.WordsHelper.ToSimplifiedChinese(subtitle.Content),
                    SubtitleConvertType.ToTraditionalChinese => ToolGood.Words.WordsHelper.ToTraditionalChinese(subtitle.Content),
                    _ => subtitle.Content
                }
                : string.Empty;
        }

        partial void OnConvertTypeChanged(SubtitleConvertType value)
            => _settingsToolkit.WriteLocalSetting(SettingNames.SubtitleConvertType, value);

        partial void OnCanShowSubtitleChanged(bool value)
            => _settingsToolkit.WriteLocalSetting(SettingNames.CanShowSubtitle, value);
    }
}
