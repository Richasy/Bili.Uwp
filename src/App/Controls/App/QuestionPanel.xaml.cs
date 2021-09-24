// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 问题面板.
    /// </summary>
    public sealed partial class QuestionPanel : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(HelpViewModel), typeof(QuestionPanel), new PropertyMetadata(HelpViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionPanel"/> class.
        /// </summary>
        public QuestionPanel()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public HelpViewModel ViewModel
        {
            get { return (HelpViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CurrentQuestionModule == null)
            {
                await ViewModel.InitializeQuestionsAsync();
            }
        }
    }
}
