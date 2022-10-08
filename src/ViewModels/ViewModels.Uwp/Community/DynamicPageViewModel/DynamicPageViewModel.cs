// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 动态页面视图模型.
    /// </summary>
    public sealed partial class DynamicPageViewModel : ViewModelBase, IDynamicPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicPageViewModel"/> class.
        /// </summary>
        public DynamicPageViewModel(
            IDynamicVideoModuleViewModel videoModule,
            IDynamicAllModuleViewModel allModule,
            IResourceToolkit resourceToolkit,
            IAuthorizeProvider authorizeProvider)
        {
            VideoModule = videoModule;
            AllModule = allModule;
            _resourceToolkit = resourceToolkit;
            _authorizeProvider = authorizeProvider;

            Headers = new ObservableCollection<DynamicHeader>
            {
                new DynamicHeader(true, _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.Videos)),
                new DynamicHeader(false, _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.ComprehensiveDynamics)),
            };

            var canInteraction = this.WhenAnyValue(p => p.NeedSignIn)
                .Select(p => !p);
            SelectHeaderCommand = new RelayCommand<DynamicHeader>(SelectHeader, canInteraction, RxApp.MainThreadScheduler);
            RefreshModuleCommand = new RelayCommand(RefreshModule, canInteraction, RxApp.MainThreadScheduler);

            NeedSignIn = _authorizeProvider.State != Models.Enums.AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;
            SelectHeader(Headers.First());
        }

        private void RefreshModule()
        {
            if (CurrentHeader.IsVideo)
            {
                VideoModule.ReloadCommand.Execute().Subscribe();
            }
            else
            {
                AllModule.ReloadCommand.Execute().Subscribe();
            }
        }

        private void SelectHeader(DynamicHeader header)
        {
            CurrentHeader = header;
            IsShowVideo = header.IsVideo;
            IsShowAll = !header.IsVideo;
        }

        private void OnAuthorizeStateChanged(object sender, AuthorizeStateChangedEventArgs e)
        {
            NeedSignIn = e.NewState != Models.Enums.AuthorizeState.SignedIn;
            RefreshModuleCommand.Execute().Subscribe();
        }
    }
}
