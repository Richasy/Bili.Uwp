// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.Popup
{
    static class Program
    {
        [STAThread]
        static void Main(string[] _)
        {
            PopupUI.App xamlApp = new();
            App app = new();
            app.InitializeComponent();
            app.Run();
        }
    }
}
