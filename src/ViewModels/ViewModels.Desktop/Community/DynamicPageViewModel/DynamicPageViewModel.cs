// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Community
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

            SelectHeaderCommand = new RelayCommand<DynamicHeader>(SelectHeader, _ => !NeedSignIn);
            RefreshModuleCommand = new RelayCommand(RefreshModule, () => !NeedSignIn);

            NeedSignIn = _authorizeProvider.State != Models.Enums.AuthorizeState.SignedIn;
            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;
            SelectHeader(Headers.First());
        }

        private void RefreshModule()
        {
            if (CurrentHeader.IsVideo)
            {
                VideoModule.ReloadCommand.ExecuteAsync(null);
            }
            else
            {
                AllModule.ReloadCommand.ExecuteAsync(null);
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
            RefreshModuleCommand.Execute(null);
        }
    }
}
