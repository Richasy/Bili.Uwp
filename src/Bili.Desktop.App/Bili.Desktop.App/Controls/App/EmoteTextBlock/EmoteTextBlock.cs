// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bili.Models.Data.Appearance;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Bili.Desktop.App.Controls.App
{
    /// <summary>
    /// 带表情的文本.
    /// </summary>
    public sealed class EmoteTextBlock : Control
    {
        /// <summary>
        /// <see cref="MaxLines"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty MaxLinesProperty =
            DependencyProperty.Register(nameof(MaxLines), typeof(int), typeof(EmoteTextBlock), new PropertyMetadata(4));

        /// <summary>
        /// <see cref="Text"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(EmoteText), typeof(EmoteTextBlock), new PropertyMetadata(default, new PropertyChangedCallback(OnTextChanged)));

        private RichTextBlock _richBlock;
        private RichTextBlock _flyoutRichBlock;
        private Button _overflowButton;
        private bool _isOverflowInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoteTextBlock"/> class.
        /// </summary>
        public EmoteTextBlock() => DefaultStyleKey = typeof(EmoteTextBlock);

        /// <summary>
        /// 最大行数.
        /// </summary>
        public int MaxLines
        {
            get { return (int)GetValue(MaxLinesProperty); }
            set { SetValue(MaxLinesProperty, value); }
        }

        /// <summary>
        /// 输入的文本.
        /// </summary>
        public EmoteText Text
        {
            get { return (EmoteText)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// 重置状态.
        /// </summary>
        public void Reset() => _richBlock?.Blocks?.Clear();

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _richBlock = GetTemplateChild("RichBlock") as RichTextBlock;
            _flyoutRichBlock = GetTemplateChild("FlyoutRichBlock") as RichTextBlock;
            _overflowButton = GetTemplateChild("OverflowButton") as Button;

            _richBlock.IsTextTrimmedChanged += OnIsTextTrimmedChanged;
            _overflowButton.Click += OnOverflowButtonClick;

            InitializeContent();
            base.OnApplyTemplate();
        }

        private static void OnReplyInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as EmoteTextBlock;
            instance.Text = null;
            if (e.NewValue != null)
            {
                instance.InitializeContent();
            }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as EmoteTextBlock;
            if (e.NewValue != null)
            {
                instance.InitializeContent();
            }
        }

        private void OnIsTextTrimmedChanged(RichTextBlock sender, IsTextTrimmedChangedEventArgs args)
            => _overflowButton.Visibility = sender.IsTextTrimmed ? Visibility.Visible : Visibility.Collapsed;

        private void InitializeContent()
        {
            _isOverflowInitialized = false;
            if (_richBlock != null)
            {
                _richBlock.Blocks.Clear();
                Paragraph para = null;
                if (Text != null)
                {
                    para = ParseText();
                }

                if (para != null)
                {
                    _richBlock.Blocks.Add(para);
                }
            }

            if (_overflowButton != null && _richBlock != null)
            {
                _overflowButton.Visibility = _richBlock.IsTextTrimmed ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnOverflowButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_isOverflowInitialized)
            {
                _flyoutRichBlock.Blocks.Clear();

                if (Text != null)
                {
                    var para = ParseText();
                    _flyoutRichBlock.Blocks.Add(para);
                }
            }
        }

        private Paragraph ParseText()
        {
            var text = Text.Text;
            var emotes = Text.Emotes;
            var para = new Paragraph();

            if (emotes != null && emotes.Count > 0)
            {
                // 有表情存在，进行处理.
                var emotiRegex = new Regex(@"(\[.*?\])");
                var splitCotents = emotiRegex.Split(text).Where(p => p.Length > 0).ToArray();
                foreach (var content in splitCotents)
                {
                    if (emotiRegex.IsMatch(content))
                    {
                        emotes.TryGetValue(content, out var emoji);
                        if (emoji != null)
                        {
                            var inlineCon = new InlineUIContainer();
                            var img = new Microsoft.UI.Xaml.Controls.Image() { Width = 20, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(2, 0, 2, -4) };
                            var bitmap = new BitmapImage(new Uri(emoji.Uri)) { DecodePixelWidth = 40 };
                            img.Source = bitmap;
                            inlineCon.Child = img;
                            para.Inlines.Add(inlineCon);
                        }
                        else
                        {
                            para.Inlines.Add(new Run { Text = content });
                        }
                    }
                    else
                    {
                        para.Inlines.Add(new Run { Text = content });
                    }
                }
            }
            else
            {
                para.Inlines.Add(new Run { Text = text });
            }

            return para;
        }
    }
}
