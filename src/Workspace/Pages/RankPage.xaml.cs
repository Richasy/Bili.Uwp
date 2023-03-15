// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.ViewModels.Interfaces.Home;

namespace Bili.Workspace.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RankPage : RankPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RankPage"/> class.
        /// </summary>
        public RankPage()
        {
            InitializeComponent();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => ViewModel.InitializeCommand.Execute(default);

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsReloading))
            {
                ContentScrollViewer.ChangeView(0, 0, 1);
            }
        }

        private void OnErrorPanelButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
            => ViewModel.ReloadCommand.Execute(default);
    }

    /// <summary>
    /// <see cref="RankPage"/>的基类.
    /// </summary>
    public class RankPageBase : PageBase<IRankPageViewModel>
    {
    }
}
