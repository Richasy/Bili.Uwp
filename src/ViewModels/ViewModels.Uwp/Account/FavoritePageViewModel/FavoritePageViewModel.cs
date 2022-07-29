// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Bili.Models.App.Other;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 收藏页视图模型.
    /// </summary>
    public sealed partial class FavoritePageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoritePageViewModel"/> class.
        /// </summary>
        public FavoritePageViewModel(
            IResourceToolkit resourceToolkit,
            IAppViewModel appViewModel)
        {
            _resourceToolkit = resourceToolkit;
            _appViewModel = appViewModel;

            TypeCollection = new ObservableCollection<FavoriteHeader>()
            {
                CreateHeader(FavoriteType.Video),
                CreateHeader(FavoriteType.Anime),
                CreateHeader(FavoriteType.Cinema),
            };

            if (!_appViewModel.IsXbox)
            {
                TypeCollection.Add(CreateHeader(FavoriteType.Article));
            }

            SelectTypeCommand = ReactiveCommand.Create<FavoriteHeader>(SelectType);
            SelectType(TypeCollection.First());
        }

        private void SelectType(FavoriteHeader type)
        {
            CurrentType = type;
            IsVideoShown = type.Type == FavoriteType.Video;
            IsAnimeShown = type.Type == FavoriteType.Anime;
            IsCinemaShown = type.Type == FavoriteType.Cinema;
            IsArticleShown = type.Type == FavoriteType.Article;
        }

        private FavoriteHeader CreateHeader(FavoriteType type)
        {
            var title = type switch
            {
                FavoriteType.Video => _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Video),
                FavoriteType.Anime => _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Anime),
                FavoriteType.Cinema => _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Cinema),
                FavoriteType.Article => _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SpecialColumn),
                _ => throw new NotImplementedException(),
            };

            return new FavoriteHeader(type, title);
        }
    }
}
