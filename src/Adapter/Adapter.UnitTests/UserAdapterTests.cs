// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Bili.Adapter.UnitTests
{
    public class UserAdapterTests
    {
        private const string UserName = "Richasy";
        private const string Avatar = "https://xxx.png";
        private const int UserId = 1;
        private const int SmallSize = 48;
        private const int LargeSize = 96;

        private readonly IImageAdapter _imageAdapter;
        private readonly UserAdapter _userAdapter;

        public UserAdapterTests()
        {
            var imageMock = new Mock<IImageAdapter>();
            imageMock.Setup(_ => _.ConvertToImage(Avatar)).Returns(new Models.Data.Appearance.Image(Avatar));
            imageMock.Setup(_ => _.ConvertToImage(Avatar, SmallSize, SmallSize)).Returns(new Models.Data.Appearance.Image(Avatar, SmallSize, SmallSize, (w, h) => $"@{w}w_{h}h.jpg"));
            imageMock.Setup(_ => _.ConvertToImage(Avatar, LargeSize, LargeSize)).Returns(new Models.Data.Appearance.Image(Avatar, LargeSize, LargeSize, (w, h) => $"@{w}w_{h}h.jpg"));
            _imageAdapter = imageMock.Object;
            _userAdapter = new UserAdapter(_imageAdapter);
        }

        [Fact]
        public void Test_BasicUserProfile()
        {
            var user = _userAdapter.ConvertToUserProfile(UserId, UserName, Avatar, true);
            user.Avatar.Width.Should().Be(SmallSize);
            user.Avatar.Height.Should().Be(SmallSize);
            user.Avatar.GetSourceUri().Should().Be(Avatar);
            user.Id.Should().Be(UserId.ToString());
            user.Name.Should().Be(UserName);
        }

        [Fact]
        public void Test_LargeAvatarUserProfile()
        {
            var user = _userAdapter.ConvertToUserProfile(UserId, UserName, Avatar, false);
            user.Avatar.Width.Should().Be(LargeSize);
            user.Avatar.Height.Should().Be(LargeSize);
        }
    }
}
