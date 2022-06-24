// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.Models.Data.Appearance;
using Bili.Models.Enums;
using Bili.ViewModels.Uwp.Pgc;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// PGC索引页面.
    /// </summary>
    public sealed partial class PgcIndexPage : PgcIndexPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcIndexPage"/> class.
        /// </summary>
        public PgcIndexPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PgcType type)
            {
                ViewModel.SetType(type);
            }
        }

        private void OnConditionChangedAsync(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox.DataContext is IndexFilterViewModel source
                && comboBox.SelectedItem is Condition item)
            {
                var index = source.Data.Conditions.ToList().IndexOf(item);
                if (index >= 0 && index != source.SelectedIndex)
                {
                    source.SelectedIndex = index;
                    ViewModel.ReloadCommand.Execute().Subscribe();
                }
            }
        }
    }

    /// <summary>
    /// <see cref="PgcIndexPage"/> 的基类.
    /// </summary>
    public class PgcIndexPageBase : AppPage<IndexPageViewModel>
    {
    }
}
