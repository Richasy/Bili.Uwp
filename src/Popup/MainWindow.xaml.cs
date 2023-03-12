// Copyright (c) Richasy. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using MicaWPF.Controls;

namespace Popup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : MicaWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Top = SystemParameters.WorkArea.Height;
        }

        /// <inheritdoc/>
        protected override void OnActivated(EventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            NativeMethods.SetWindowLong(
                hwnd,
                NativeMethods.GWLSTYLE,
                NativeMethods.GetWindowLong(hwnd, NativeMethods.GWLSTYLE) & ~NativeMethods.WSSYSMENU);
            Width = 450;
            Height = 720;
            Left = (SystemParameters.WorkArea.Width - Width) / 2;
            var targetTop = SystemParameters.WorkArea.Height - Height - 12;
            var dbAni = new DoubleAnimation(SystemParameters.WorkArea.Height, targetTop, (Duration)TimeSpan.FromSeconds(0.15));
            BeginAnimation(TopProperty, dbAni);
            base.OnActivated(e);
        }

        /// <inheritdoc/>
        protected override void OnDeactivated(EventArgs e)
        {
            var dbAni = new DoubleAnimation(Top, SystemParameters.WorkArea.Height, (Duration)TimeSpan.FromSeconds(0.15));
            dbAni.Completed += (_, _) =>
            {
                Close();
            };
            BeginAnimation(TopProperty, dbAni);
            base.OnDeactivated(e);
        }

        private class NativeMethods
        {
            internal const int GWLSTYLE = -16;
            internal const int WSSYSMENU = 0x80000;

            [DllImport("user32.dll", SetLastError = true)]
            internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll")]
            internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        }
    }
}
