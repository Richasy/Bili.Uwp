// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using Bili.ViewModels.Interfaces;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 互动视频模块视图模型.
    /// </summary>
    public sealed partial class InteractionModuleViewModel : ViewModelBase, IReloadViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionModuleViewModel"/> class.
        /// </summary>
        public InteractionModuleViewModel(
            IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
            Choices = new ObservableCollection<InteractionInformation>();
            ReloadCommand = ReactiveCommand.CreateFromTask(ReloadAsync);

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading);
            ReloadCommand.ThrownExceptions.ObserveOn(RxApp.MainThreadScheduler).Subscribe(LogException);
        }

        /// <summary>
        /// 设置初始数据.
        /// </summary>
        /// <param name="partId">视频分P Id.</param>
        /// <param name="choiceId">选项 Id.</param>
        /// <param name="graphVersion">互动版本.</param>
        public void SetData(string partId, string choiceId, string graphVersion)
        {
            _partId = partId;
            _choiceId = string.IsNullOrEmpty(choiceId) ? "0" : choiceId;
            _graphVersion = graphVersion;
            ReloadCommand.Execute().Subscribe();
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
