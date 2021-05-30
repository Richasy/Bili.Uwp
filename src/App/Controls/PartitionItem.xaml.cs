// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Models.BiliBili;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            DependencyProperty.Register(nameof(Data), typeof(Partition), typeof(PartitionItem), new PropertyMetadata(null, new PropertyChangedCallback(OnDataChanged)));

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
        public event EventHandler<Partition> ItemClick;

        /// <summary>
        /// 分区数据.
        /// </summary>
        public Partition Data
        {
            get { return (Partition)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as PartitionItem;
            if (e.NewValue != null && e.NewValue is Partition data)
            {
                instance.PartitionLogo.Source = new BitmapImage(new Uri(data.Logo));
                instance.PartitionName.Text = data.Name;
            }
        }

        private void OnItemTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            e.Handled = true;
            ItemClick?.Invoke(this, Data);
        }
    }
}
