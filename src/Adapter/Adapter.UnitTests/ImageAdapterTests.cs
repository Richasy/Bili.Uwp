// Copyright (c) Richasy. All rights reserved.

using FluentAssertions;
using Xunit;

namespace Bili.Adapter.UnitTests
{
    public class ImageAdapterTests
    {
        private const string ImageUrl = "https://xxx.com/11111.png";

        [Fact]
        public void Test_CorrectUrl()
        {
            var adapter = new ImageAdapter();
            var image = adapter.ConvertToImage(ImageUrl);
            image.Uri.ToString().Should().Be(ImageUrl);
        }

        [Fact]
        public void Test_CorrectSize()
        {
            var adapter = new ImageAdapter();
            var width = 400;
            var height = 300;
            var image = adapter.ConvertToImage(ImageUrl, width, height);
            image.Uri.ToString().Should().Contain("400w");
            image.Uri.ToString().Should().Contain("300h");
        }
    }
}
