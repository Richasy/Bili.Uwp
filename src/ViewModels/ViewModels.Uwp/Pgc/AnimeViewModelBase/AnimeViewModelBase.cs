// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 动漫视图模型基类.
    /// </summary>
    public partial class AnimeViewModelBase : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimeViewModelBase"/> class.
        /// </summary>
        /// <param name="type">PGC类型.</param>
        public AnimeViewModelBase(PgcType type)
        {
            Type = type;
            TabCollection = new ObservableCollection<PgcTabViewModel>();
            PropertyChanged += OnPropertyChangedAsync;
        }

        /// <summary>
        /// 初始化请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading)
            {
                IsInitializeLoading = true;
                IsError = false;
                try
                {
                    var tabs = await Controller.RequestPgcTabsAsync(Type);
                    tabs.ForEach(p => TabCollection.Add(new PgcTabViewModel(p)));
                    CurrentTab = TabCollection.First();
                    IsRequested = true;
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestPgcFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }
        }

        private async void OnPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentTab))
            {
                CheckTagActivate();
                if (!CurrentTab.IsRequested)
                {
                    await CurrentTab.InitializeRequestAsync();
                }
            }
        }

        private void CheckTagActivate()
        {
            foreach (var tab in TabCollection)
            {
                if (tab.Id == CurrentTab.Id)
                {
                    if (!tab.IsActivate)
                    {
                        tab.Activate();
                    }
                }
                else
                {
                    tab.Deactive();
                }
            }
        }
    }
}
