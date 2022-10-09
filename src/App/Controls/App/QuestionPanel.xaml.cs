// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Home;

namespace Bili.App.Controls
{
    /// <summary>
    /// 问题面板.
    /// </summary>
    public sealed partial class QuestionPanel : QuestionPanelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionPanel"/> class.
        /// </summary>
        public QuestionPanel()
        {
            InitializeComponent();
            ViewModel = Locator.Instance.GetService<IHelpPageViewModel>();
        }
    }

    /// <summary>
    /// <see cref="QuestionPanel"/> 的基类.
    /// </summary>
    public class QuestionPanelBase : ReactiveUserControl<IHelpPageViewModel>
    {
    }
}
