// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.DI.Container;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Microsoft.UI.Xaml;

namespace Bili.Workspace.Controls.Dynamic
{
    /// <summary>
    /// 动态模块.
    /// </summary>
    public sealed partial class DynamicModule : DynamicModuleBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicModule"/> class.
        /// </summary>
        public DynamicModule() => InitializeComponent();

        internal override void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is IDynamicModuleViewModel oldVM)
            {
                oldVM.PropertyChanged -= OnViewModelPropertyChanged;
            }

            if (e.NewValue is IDynamicModuleViewModel newVM)
            {
                newVM.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsReloading))
            {
                ContentScrollViewer.ChangeView(0, 0, 1);
            }
        }

        private void OnVideoViewRequestLoadMore(object sender, System.EventArgs e)
            => ViewModel.IncrementalCommand.Execute(default);
    }

    /// <summary>
    /// <see cref="DynamicModule"/>的基类.
    /// </summary>
    public class DynamicModuleBase : ReactiveUserControl<IDynamicModuleViewModel>
    {
    }
}
