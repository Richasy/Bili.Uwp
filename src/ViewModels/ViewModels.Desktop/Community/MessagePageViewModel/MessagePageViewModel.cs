// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop.Base;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Community;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 消息页面视图模型.
    /// </summary>
    public sealed partial class MessagePageViewModel : InformationFlowViewModelBase<IMessageItemViewModel>, IMessagePageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePageViewModel"/> class.
        /// </summary>
        public MessagePageViewModel(
            IAccountProvider accountProvider,
            IResourceToolkit resourceToolkit,
            IAccountViewModel accountViewModel)
        {
            _accountProvider = accountProvider;
            _resourceToolkit = resourceToolkit;
            _accountViewModel = accountViewModel;

            _caches = new Dictionary<MessageType, (IEnumerable<MessageInformation> Items, bool IsEnd)>();
            MessageTypes = new ObservableCollection<IMessageHeaderViewModel>
            {
                GetMessageHeader(MessageType.Reply),
                GetMessageHeader(MessageType.At),
                GetMessageHeader(MessageType.Like),
            };

            InitializeMessageCount();

            SelectTypeCommand = new AsyncRelayCommand<IMessageHeaderViewModel>(SelectTypeAsync);
            _accountViewModel.PropertyChanged += OnAccountViewModelPropertyChanged;
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _accountProvider.ClearMessageStatus();
            if (_shouldClearCache)
            {
                _caches.Clear();
            }

            _isEnd = false;
            IsEmpty = false;
            _shouldClearCache = true;
        }

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (_caches.Count == 0)
            {
                await FakeLoadingAsync();
                CurrentType = MessageTypes.First();
            }

            if (_isEnd)
            {
                return;
            }

            var view = await _accountProvider.GetMyMessagesAsync(CurrentType.Type);
            _isEnd = view.IsFinished;
            foreach (var item in view.Messages)
            {
                var messageVM = Locator.Instance.GetService<IMessageItemViewModel>();
                messageVM.InjectData(item);
                Items.Add(messageVM);
            }

            _caches.Remove(CurrentType.Type);
            _caches.Add(CurrentType.Type, new(Items.Select(p => p.Data).ToList(), _isEnd));
            IsEmpty = Items.Count == 0;
            _ = _accountViewModel.InitializeUnreadCommand.ExecuteAsync(null);
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestMessageFailed)}\n{errorMsg}";

        private async Task SelectTypeAsync(IMessageHeaderViewModel type)
        {
            await FakeLoadingAsync();
            TryClear(Items);
            _isEnd = false;
            CurrentType = type;
            if (_caches.TryGetValue(CurrentType.Type, out var data) && data.Items.Count() > 0)
            {
                foreach (var item in data.Items)
                {
                    var messageVM = Locator.Instance.GetService<IMessageItemViewModel>();
                    messageVM.InjectData(item);
                    Items.Add(messageVM);
                }

                _isEnd = data.IsEnd;
                IsEmpty = Items.Count == 0;
            }
            else
            {
                _shouldClearCache = false;
                _ = InitializeCommand.ExecuteAsync(null);
            }
        }

        private void InitializeMessageCount()
        {
            var unreadInfo = _accountViewModel.UnreadInformation;
            if (unreadInfo != null)
            {
                MessageTypes.First(p => p.Type == MessageType.Reply).Count = unreadInfo.ReplyCount;
                MessageTypes.First(p => p.Type == MessageType.At).Count = unreadInfo.AtCount;
                MessageTypes.First(p => p.Type == MessageType.Like).Count = unreadInfo.LikeCount;
            }
            else
            {
                foreach (var item in MessageTypes)
                {
                    item.Count = 0;
                }
            }
        }

        private IMessageHeaderViewModel GetMessageHeader(MessageType type)
        {
            var vm = Locator.Instance.GetService<IMessageHeaderViewModel>();
            vm.SetData(type);
            return vm;
        }

        private void OnAccountViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_accountViewModel.UnreadInformation))
            {
                InitializeMessageCount();
            }
        }
    }
}
