// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Pgc;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Base
{
    /// <summary>
    /// PGC收藏夹视图模型.
    /// </summary>
    public partial class PgcFavoriteModuleViewModelBase : InformationFlowViewModelBase<ISeasonItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcFavoriteModuleViewModelBase"/> class.
        /// </summary>
        internal PgcFavoriteModuleViewModelBase(
            FavoriteType type,
            IFavoriteProvider favoriteProvider,
            IResourceToolkit resourceToolkit)
        {
            _type = type;
            _favoriteProvider = favoriteProvider;
            _resourceToolkit = resourceToolkit;
            StatusCollection = new ObservableCollection<int> { 1, 2, 3 };
            Status = 2;

            SetStatusCommand = new RelayCommand<int>(SetStatus);
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _isEnd = false;
            IsEmpty = false;
            if (_type == FavoriteType.Anime)
            {
                _favoriteProvider.ResetAnimeStatus();
            }
            else if (_type == FavoriteType.Cinema)
            {
                _favoriteProvider.ResetCinemaStatus();
            }
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
        {
            var prefix = _type == FavoriteType.Anime
                ? LanguageNames.RequestAnimeFavoriteFailed
                : LanguageNames.RequestCinemaFavoriteFailed;
            return $"{_resourceToolkit.GetLocaleString(prefix)}\n{errorMsg}";
        }

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (_isEnd)
            {
                return;
            }

            var data = _type == FavoriteType.Anime
                ? await _favoriteProvider.GetFavoriteAnimeListAsync(Status)
                : await _favoriteProvider.GetFavoriteCinemaListAsync(Status);

            foreach (var item in data.Items)
            {
                var seasonVM = Locator.Instance.GetService<ISeasonItemViewModel>();
                seasonVM.InjectData(item);
                seasonVM.InjectAction(vm => RemoveItem(vm));
                Items.Add(seasonVM);
            }

            IsEmpty = Items.Count == 0;
            _isEnd = Items.Count >= data.TotalCount;
        }

        private void SetStatus(int status)
        {
            Status = status;
            TryClear(Items);
            InitializeCommand.ExecuteAsync(null);
        }

        private void RemoveItem(ISeasonItemViewModel vm)
        {
            Items.Remove(vm);
            IsEmpty = Items.Count == 0;
        }
    }
}
