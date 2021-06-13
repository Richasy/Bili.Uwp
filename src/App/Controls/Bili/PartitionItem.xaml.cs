// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 分区条目.
    /// </summary>
    public sealed partial class PartitionItem : UserControl
    {
        /// <summary>
        /// <see cref="Data"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(PartitionViewModel), typeof(PartitionItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionItem"/> class.
        /// </summary>
        public PartitionItem()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在条目被点击时发生.
        /// </summary>
        public event EventHandler<PartitionViewModel> ItemClick;

        /// <summary>
        /// 分区数据.
        /// </summary>
        public PartitionViewModel Data
        {
            get { return (PartitionViewModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as PartitionItem;
            if (e.NewValue != null && e.NewValue is PartitionViewModel data)
            {
                instance.PartitionLogo.Source = new BitmapImage(new Uri(data.ImageUrl));
                instance.PartitionName.Text = data.Title;
            }
        }

        private void OnItemTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            e.Handled = true;
            var animationService = ConnectedAnimationService.GetForCurrentView();
            animationService.PrepareToAnimate("PartitionLogoAnimate", this.PartitionLogo);
            animationService.PrepareToAnimate("PartitionNameAnimate", this.PartitionName);
            ItemClick?.Invoke(this, Data);
        }
    }
}
