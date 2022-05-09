// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using FluentAssertions;
using Richasy.Adapter;
using Xunit;

namespace Bili.Adapter.UnitTests
{
    public class CommunityAdapterTests
    {
        private const int LikeCount = 100;
        private const int CoinCount = 200;
        private const int DynamicCount = 300;
        private const int FollowCount = 10;
        private const int FansCount = 20;
        private const int UserId = 1;
        private const int InvalidNumber = -1;

        private readonly ICommunityAdapter _communityAdapter;

        public CommunityAdapterTests()
            => _communityAdapter = new CommunityAdapter();

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
    }
}
