// Copyright (c) GodLeaveMe. All rights reserved.

namespace Richasy.Bili.Models.App.Constants
{
    /// <summary>
    /// 控制器使用的常量.
    /// </summary>
    public static class ControllerConstants
    {
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Fields should be private
        public static class Names
        {
            public const string PartitionIndex = "partitionIndex.json";
            public const string DocumentaryCategories = "documentaryCategories.json";
            public const string ServerFolder = "Server";
            public const string LoggerFolder = "Logger";
            public const string LoggerName = "AppLog.log";
        }

        public static class Search
        {
            public const string OrderType = "orderType";
            public const string OrderSort = "orderSort";
            public const string Duration = "duration";
            public const string PartitionId = "partitionId";
            public const string TotalRank = "totalrank";
            public const string UserType = "userType";
        }

        public static class Live
        {
            public const string DanmakuMessage = "DANMU_MSG";
            public const string SendGiftMessage = "SEND_GIFT";
            public const string ComboSendGiftMessage = "COMBO_SEND";
            public const string WelcomeMessage = "WELCOME";
            public const string SystemMessage = "SYS_MSG";
            public const string AnchorLotteryStartMessage = "ANCHOR_LOT_START";
            public const string AnchorLotteryAwardMessage = "ANCHOR_LOT_AWARD";
            public const string SuperChatMessage = "SUPER_CHAT_MESSAGE";
        }
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1401 // Fields should be private
    }
}
