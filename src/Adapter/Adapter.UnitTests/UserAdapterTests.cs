// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bilibili.App.View.V1;
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
        private const int AvatarSize = 48;

        private readonly IImageAdapter _imageAdapter;
        private readonly UserAdapter _userAdapter;

        public UserAdapterTests()
        {
            var imageMock = new Mock<IImageAdapter>();
            imageMock.Setup(_ => _.ConvertToImage(Avatar)).Returns(new Models.Data.Appearance.Image(Avatar));
            imageMock.Setup(_ => _.ConvertToImage(Avatar, It.IsAny<double>(), It.IsAny<double>()))
                     .Returns(new Models.Data.Appearance.Image(Avatar, AvatarSize, AvatarSize, (w, h) => $"@{w}w_{h}h.jpg"));

            _imageAdapter = imageMock.Object;
            _userAdapter = new UserAdapter(_imageAdapter);
        }

        [Fact]
        public void ConvertToUserProfile_AvatarSize_Valid()
        {
            var user = _userAdapter.ConvertToUserProfile(UserId, UserName, Avatar, Models.Enums.App.AvatarSize.Size48);
            user.Avatar.Width.Should().Be(AvatarSize);
            user.Avatar.Height.Should().Be(AvatarSize);
            user.Avatar.GetSourceUri().Should().Be(Avatar);
            user.Id.Should().Be(UserId.ToString());
            user.Name.Should().Be(UserName);
        }

        [Fact]
        public void ConvertToRoleProfile_Staff_Valid()
        {
            var role = "参演";
            var staff = new Staff
            {
                Face = Avatar,
                Mid = UserId,
                Name = UserName,
                Title = role,
            };
            var publisher = _userAdapter.ConvertToRoleProfile(staff, Models.Enums.App.AvatarSize.Size48);

            publisher.Should().NotBeNull();
            publisher.User.Should().NotBeNull();
            publisher.User.Id.Should().Be(UserId.ToString());
            publisher.User.Name.Should().Be(UserName);
            publisher.User.Avatar.GetSourceUri().Should().Be(Avatar);
            publisher.Role.Should().Be(role);
        }

        [Fact]
        public void ConvertToRoleProfile_PublisherInfo_Valid()
        {
            var role = "Publisher";
            var staff = new PublisherInfo
            {
                PublisherAvatar = Avatar,
                Mid = UserId,
                Publisher = UserName,
            };
            var publisher = _userAdapter.ConvertToRoleProfile(staff, Models.Enums.App.AvatarSize.Size48);

            publisher.Should().NotBeNull();
            publisher.User.Should().NotBeNull();
            publisher.User.Id.Should().Be(UserId.ToString());
            publisher.User.Name.Should().Be(UserName);
            publisher.User.Avatar.GetSourceUri().Should().Be(Avatar);
            publisher.Role.Should().Be(role);
        }

        [Fact]
        public void ConvertToRoleProfile_RecommendAvatar_Valid()
        {
            var avatar = new RecommendAvatar
            {
                Cover = Avatar,
                UserId = UserId,
                UserName = UserName,
            };
            var publisher = _userAdapter.ConvertToRoleProfile(avatar);

            publisher.Should().NotBeNull();
            publisher.User.Should().NotBeNull();
            publisher.User.Id.Should().Be(UserId.ToString());
            publisher.User.Name.Should().Be(UserName);
            publisher.User.Avatar.GetSourceUri().Should().Be(Avatar);
        }

        [Fact]
        public void ConvertToAccountInformation_UserSpaceInformation_Valid()
        {
            var sign = "Hello world";
            var level = 1;
            var spaceInfo = new UserSpaceInformation
            {
                UserId = UserId.ToString(),
                UserName = UserName,
                Avatar = Avatar,
                Sign = sign,
                Vip = new Models.BiliBili.Vip { Status = 1 },
                LevelInformation = new UserSpaceLevelInformation { CurrentLevel = level },
            };

            var account = _userAdapter.ConvertToAccountInformation(spaceInfo, Models.Enums.App.AvatarSize.Size48);
            account.Should().NotBeNull();
            account.User.Should().NotBeNull();
            account.User.Id.Should().Be(UserId.ToString());
            account.User.Name.Should().Be(UserName);
            account.User.Avatar.GetSourceUri().Should().Be(Avatar);
            account.Introduce.Should().Be(sign);
            account.IsVip.Should().BeTrue();
            account.Level.Should().Be(level);
        }

        [Fact]
        public void ConvertToAccountInformation_MyInfo_Valid()
        {
            var sign = "Hello world";
            var level = 1;
            var myInfo = new MyInfo
            {
                Mid = UserId,
                Name = UserName,
                Avatar = Avatar,
                Sign = sign,
                VIP = new Models.BiliBili.Vip { Status = 1 },
                Level = level,
            };

            var account = _userAdapter.ConvertToAccountInformation(myInfo, Models.Enums.App.AvatarSize.Size48);
            account.Should().NotBeNull();
            account.User.Should().NotBeNull();
            account.User.Id.Should().Be(UserId.ToString());
            account.User.Name.Should().Be(UserName);
            account.User.Avatar.GetSourceUri().Should().Be(Avatar);
            account.Introduce.Should().Be(sign);
            account.IsVip.Should().BeTrue();
            account.Level.Should().Be(level);
        }
    }
}
