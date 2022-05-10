// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Toolkit.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Bili.Adapter.UnitTests
{
    public class CommunityAdapterTests
    {
        private const string InteractionCountText = "4.5万xx";
        private const int InteractionCount = 45000;
        private const string RecommendReasonText = "百万播放";

        private const int VideoId = 111;
        private const int LikeCount = 100;
        private const int CoinCount = 200;
        private const int DynamicCount = 300;
        private const int FollowCount = 10;
        private const int FansCount = 20;
        private const int UserId = 1;
        private const int InvalidNumber = -1;

        private readonly ICommunityAdapter _communityAdapter;

        public CommunityAdapterTests()
        {
            var numberToolkitMock = new Mock<INumberToolkit>();
            numberToolkitMock.Setup(_ => _.GetCountNumber(InteractionCountText, It.IsAny<string>())).Returns(InteractionCount);

            _communityAdapter = new CommunityAdapter(numberToolkitMock.Object);
        }

        [Fact]
        public void ConvertToUserCommunityInformation_Mine_Valid()
        {
            var mine = new Mine
            {
                CoinNumber = CoinCount,
                DynamicCount = DynamicCount,
                FollowCount = FollowCount,
                FollowerCount = FansCount,
                Mid = UserId,
            };

            var data = _communityAdapter.ConvertToUserCommunityInformation(mine);
            data.Should().NotBeNull();
            data.CoinCount.Should().Be(CoinCount);
            data.FansCount.Should().Be(FansCount);
            data.DynamicCount.Should().Be(DynamicCount);
            data.FollowCount.Should().Be(FollowCount);
            data.Id.Should().Be(UserId.ToString());

            data.LikeCount.Should().Be(InvalidNumber);
        }

        [Fact]
        public void ConvertToUserCommunityInformation_UserSpaceInformation_Valid()
        {
            var mine = new UserSpaceInformation
            {
                LikeInformation = new UserSpaceLikeInformation { LikeCount = LikeCount },
                FollowCount = FollowCount,
                FollowerCount = FansCount,
                UserId = UserId.ToString(),
            };

            var data = _communityAdapter.ConvertToUserCommunityInformation(mine);
            data.Should().NotBeNull();
            data.FansCount.Should().Be(FansCount);
            data.FollowCount.Should().Be(FollowCount);
            data.LikeCount.Should().Be(LikeCount);
            data.Id.Should().Be(UserId.ToString());

            data.DynamicCount.Should().Be(InvalidNumber);
            data.CoinCount.Should().Be(InvalidNumber);
        }

        [Fact]
        public void ConvertToVideoCommunityInformation_RecommendCard_Valid()
        {
            var recommendCard = new RecommendCard
            {
                CardGoto = "av",
                PlayCountText = InteractionCountText,
                SubStatusText = InteractionCountText,
                RecommendReason = RecommendReasonText,
                Parameter = VideoId.ToString(),
            };

            var videoCommunity = _communityAdapter.ConvertToVideoCommunityInformation(recommendCard);
            videoCommunity.Should().NotBeNull();
            videoCommunity.PlayCount.Should().Be(InteractionCount);
            videoCommunity.DanmakuCount.Should().Be(InteractionCount);
            videoCommunity.RecommendReason.Should().Be(RecommendReasonText);
        }
    }
}
