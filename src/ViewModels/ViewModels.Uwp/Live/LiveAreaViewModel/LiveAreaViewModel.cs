// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播间分区视图模型.
    /// </summary>
    public sealed partial class LiveAreaViewModel : WebRequestViewModelBase, IDeltaRequestViewModel
    {
        internal LiveAreaViewModel()
        {
            _currentPage = 1;
            ServiceLocator.Instance.LoadService(out _resourceToolkit);
            TagCollection = new ObservableCollection<LiveAreaDetailTag>();
            LiveRoomCollection = new ObservableCollection<VideoViewModel>();

            Controller.LiveAreaRoomIteration += OnLiveAreaRoomIteration;

            this.WhenAnyValue(x => x.SelectedTag)
                .WhereNotNull()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(async x => await ChangeSelectedTagAsync());
        }

        /// <summary>
        /// 设置直播分区.
        /// </summary>
        /// <param name="area">直播分区.</param>
        public void SetLiveArea(LiveArea area)
        {
            _area = area;
            AreaImage = _area.Cover;
            AreaName = _area.Name;
            _isFirstSetTag = true;
            SelectedTag = null;
            TagCollection.Clear();
            IsRequested = false;
        }

        /// <inheritdoc/>
        public async Task RequestDataAsync()
        {
            if (_area == null)
            {
                throw new ArgumentNullException("需要先设置直播分区");
            }

            if (!IsRequested)
            {
                await InitializeRequestAsync();
            }
            else
            {
                await DeltaRequestAsync();
            }

            IsRequested = _currentPage > 1;
        }

        /// <inheritdoc/>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                _currentPage = 1;
                IsEmpty = false;
                IsError = false;
                ErrorText = string.Empty;
                LiveRoomCollection.Clear();
                _isFinish = false;

                try
                {
                    await Controller.RequestLiveAreaRoomsAsync(_area, SelectedTag?.SortType ?? string.Empty);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestLiveFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }

            IsRequested = _currentPage > 1;
        }

        internal async Task DeltaRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading && !_isFinish)
            {
                IsDeltaLoading = true;
                var sortType = SelectedTag?.SortType ?? string.Empty;
                await Controller.RequestLiveAreaRoomsAsync(_area, sortType, _currentPage);
                IsDeltaLoading = false;
            }
        }

        private void OnLiveAreaRoomIteration(object sender, LiveAreaRoomIterationEventArgs e)
        {
            if (SelectedTag != null && SelectedTag.SortType != e.SortType)
            {
                return;
            }

            var list = e.List;
            _currentPage = e.NextPageNumber;

            if (e.Tags.Count == 0)
            {
                e.Tags.Add(new LiveAreaDetailTag
                {
                    SortType = string.Empty,
                    Name = _resourceToolkit.GetLocaleString(LanguageNames.Total),
                });
            }

            if (TagCollection.Count == 0)
            {
                e.Tags.ForEach(p => TagCollection.Add(p));
                SelectedTag = TagCollection.FirstOrDefault();
            }

            foreach (var item in list)
            {
                if (!LiveRoomCollection.Any(p => p.VideoId == item.RoomId.ToString()))
                {
                    LiveRoomCollection.Add(new VideoViewModel(item));
                }
            }

            _isFinish = LiveRoomCollection.Count >= e.Count;
            IsEmpty = LiveRoomCollection.Count == 0;
        }

        private async Task ChangeSelectedTagAsync()
        {
            if (_isFirstSetTag)
            {
                _isFirstSetTag = false;
                return;
            }

            await InitializeRequestAsync();
        }
    }
}
