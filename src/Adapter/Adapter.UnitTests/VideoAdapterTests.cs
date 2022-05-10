// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Adapter.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using FluentAssertions;
using Moq;
using Xunit;

namespace Bili.Adapter.UnitTests
{
    public class VideoAdapterTests
    {
        private const string ImageUrl = "https://xxx.com/xxx.png";
        private const int VideoId = 111;
        private const string VideoTitle = "Hello world";
        private const int VideoDuration = 1000;
        private readonly VideoAdapter _videoAdapter;

        public VideoAdapterTests()
        {
            var imageAdapterMock = new Mock<IImageAdapter>();
            var userAdapterMock = new Mock<IUserAdapter>();
            var communityAdapterMock = new Mock<ICommunityAdapter>();

            imageAdapterMock.Setup(_ => _.ConvertToImage(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                            .Returns(new Models.Data.Appearance.Image(string.Empty));
            userAdapterMock.Setup(_ => _.ConvertToPublisherProfile(It.IsAny<RecommendAvatar>(), Models.Enums.App.AvatarSize.Size48)).Returns(new Models.Data.User.PublisherProfile());
            communityAdapterMock.Setup(_ => _.ConvertToVideoCommunityInformation(It.IsAny<RecommendCard>())).Returns(new Models.Data.Community.VideoCommunityInformation(VideoId.ToString()));

            _videoAdapter = new VideoAdapter(communityAdapterMock.Object, userAdapterMock.Object, imageAdapterMock.Object);
        }

        [Fact]
        public void ConvertToVideoInformation_RecommendCard_Valid()
        {
            var recommendCard = new RecommendCard
            {
                Mask = new RecommendCardMask { Avatar = new RecommendAvatar() },
                Title = VideoTitle,
                Parameter = VideoId.ToString(),
                PlayerArgs = new PlayerArgs { Duration = VideoDuration },
                Description = VideoTitle,
                Cover = ImageUrl,
                CardGoto = ServiceConstants.Av,
            };

            var video = _videoAdapter.ConvertToVideoInformation(recommendCard);
            video.Should().NotBeNull();
            video.Identifier.Should().NotBeNull();
            video.Identifier.Id.Should().Be(VideoId.ToString());
            video.Identifier.Title.Should().Be(VideoTitle);
            video.Identifier.Duration.Should().Be(VideoDuration);
        }

        [Fact]
        public void ConvertToVideoInformation_RecommendCard_CardGotoException()
        {
            var recommendCard = new RecommendCard
            {
                CardGoto = ServiceConstants.Pgc,
            };

            Assert.Throws<ArgumentException>(() => _videoAdapter.ConvertToVideoInformation(recommendCard));
        }
    }
}
