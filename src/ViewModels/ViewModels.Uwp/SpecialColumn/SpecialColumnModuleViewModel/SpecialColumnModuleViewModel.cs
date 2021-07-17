// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 专栏模块视图模型.
    /// </summary>
    public partial class SpecialColumnModuleViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialColumnModuleViewModel"/> class.
        /// </summary>
        internal SpecialColumnModuleViewModel()
        {
            _controller = BiliController.Instance;
            ServiceLocator.Instance.LoadService(out _resourceToolkit);
            CategoryCollection = new ObservableCollection<SpecialColumnCategoryViewModel>();
            PropertyChanged += OnViewModelPropertyChangedAsync;
        }

        /// <summary>
        /// 请求全部分类.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestCategoriesAsync()
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;
            IsError = false;
            CategoryCollection.Clear();
            try
            {
                var categories = await _controller.GetSpecialColumnCategoriesAsync();
                categories.Insert(0, new ArticleCategory
                {
                    Id = 0,
                    ParentId = 0,
                    Name = _resourceToolkit.GetLocaleString(LanguageNames.Recommend),
                });

                categories.ForEach(p => CategoryCollection.Add(new SpecialColumnCategoryViewModel(p)));
                CurrentCategory = CategoryCollection.First();
            }
            catch (System.Exception ex)
            {
                IsError = true;
                ErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.CategoriesReuqestFailed)}\n{ex.Message}";
            }

            IsLoading = false;
        }

        private async void OnViewModelPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentCategory))
            {
                foreach (var item in CategoryCollection)
                {
                    item.CheckActive(CurrentCategory.Id);
                }

                if (!CurrentCategory.IsRequested)
                {
                    await CurrentCategory.RequestDataAsync();
                }
            }
        }
    }
}
