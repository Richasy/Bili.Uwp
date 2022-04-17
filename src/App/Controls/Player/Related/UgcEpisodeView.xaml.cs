// Copyright (c) Richasy. All rights reserved.

using Bilibili.App.View.V1;

namespace Richasy.Bili.App.Controls.Player.Related
{
    /// <summary>
    /// 合集视图.
    /// </summary>
    public sealed partial class UgcEpisodeView : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UgcEpisodeView"/> class.
        /// </summary>
        public UgcEpisodeView()
        {
            this.InitializeComponent();
        }

        private void OnSectionComboBoxSelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            if (SectionComboBox.SelectedItem is Section item && item != ViewModel.CurrentUgcSection)
            {
                ViewModel.ChangeUgcSection(item);
            }
        }
    }
}
