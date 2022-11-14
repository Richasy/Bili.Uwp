// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Community;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace Bili.Desktop.App.Controls.Dynamic
{
    /// <summary>
    /// 动态条目.
    /// </summary>
    public sealed class DynamicItem : ReactiveControl<IDynamicItemViewModel>, IRepeaterItem, IOrientationControl
    {
        private DynamicPresenter _presenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItem"/> class.
        /// </summary>
        public DynamicItem() => DefaultStyleKey = typeof(DynamicItem);

        /// <inheritdoc/>
        public Size GetHolderSize() => new Size(300, 200);

        /// <inheritdoc/>
        public void ChangeLayout(Orientation orientation)
        {
            if (_presenter != null)
            {
                _presenter.ChangeLayout(orientation);
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
            => _presenter = GetTemplateChild("Presenter") as DynamicPresenter;
    }
}
