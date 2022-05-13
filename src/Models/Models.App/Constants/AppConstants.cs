// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.App.Constants
{
    /// <summary>
    /// 应用常量.
    /// </summary>
    public static class AppConstants
    {
#pragma warning disable SA1600 // Elements should be documented
        public const string ThemeDefault = "System";
        public const string ThemeLight = "Light";
        public const string ThemeDark = "Dark";

        public const string StartupTaskId = "Richasy.Bili";

        public const int AppMinWidth = 500;
        public const int AppMinHeight = 500;

        public const int VideoCardCoverWidth = 200;
        public const int VideoCardCoverHeight = 125;

        public const int DynamicCoverWidth = 400;
        public const int DynamicCoverHeight = 250;

        public const int PgcCoverWidth = 180;
        public const int PgcCoverHeight = 240;

        public const string DashVideoMPDFile = "ms-appx:///Assets/DashVideoTemplate.xml";
        public const string DashVideoWithoudAudioMPDFile = "ms-appx:///Assets/DashVideoWithoutAudioTemplate.xml";

        public const string ImageClickEvent = "ImageClick";
        public const string LinkClickEvent = "LinkClick";

        public const string ReplySection = "Reply";
        public const string ViewLaterSection = "ViewLater";

        public const string LastOpenVideoFileName = "LastOpenVideo.json";
        public const string FixedFolderName = "Fixed";
        public const string FixedPgcFolderName = "Fixed pgc";
        public const string FixedContentFileName = "User-{0}.json";

        public const string ScreenshotFolderName = "Bili Screenshots";

        public const string NewDynamicTaskName = "Bili.Tasks.DynamicNotifyTask";

        /// <summary>
        /// 哔哩哔哩番剧出差账户Id.
        /// </summary>
        public const int RegionalAnimeUserId = 11783021;

        public static class Protocol
        {
            public const string PlayHost = "play";
            public const string FindHost = "find";
            public const string NavigateHost = "navigate";

            public const string VideoParam = "video";
            public const string SeasonParam = "season";
            public const string EpisodeParam = "episode";
            public const string LiveParam = "live";
            public const string KeywordParam = "keyword";
            public const string IdParam = "id";
            public const string ModeParam = "mode";
            public const string IsPgcParam = "isPgc";
        }
#pragma warning restore SA1600 // Elements should be documented
    }
}
