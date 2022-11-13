// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Common
{
    /// <summary>
    /// 互动视频模块视图模型.
    /// </summary>
    public sealed partial class InteractionModuleViewModel : ViewModelBase, IInteractionModuleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionModuleViewModel"/> class.
        /// </summary>
        public InteractionModuleViewModel(
            IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
            Choices = new ObservableCollection<InteractionInformation>();
            ReloadCommand = new AsyncRelayCommand(ReloadAsync);

            AttachIsRunningToAsyncCommand(p => IsReloading = p, ReloadCommand);
            AttachExceptionHandlerToAsyncCommand(LogException, ReloadCommand);
        }

        /// <inheritdoc/>
        public void SetData(string partId, string choiceId, string graphVersion)
        {
            _partId = partId;
            _choiceId = string.IsNullOrEmpty(choiceId) ? "0" : choiceId;
            _graphVersion = graphVersion;
            ReloadCommand.ExecuteAsync(null);
        }

        private void Reset() => TryClear(Choices);

        private async Task ReloadAsync()
        {
            if (IsReloading)
            {
                return;
            }

            Reset();
            var infos = await _playerProvider.GetInteractionInformationsAsync(_partId, _graphVersion, _choiceId);
            if (infos != null)
            {
                infos.Where(p => p.IsValid).ToList().ForEach(p => Choices.Add(p));
            }

            if (Choices.Count == 0)
            {
                NoMoreChoices?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
