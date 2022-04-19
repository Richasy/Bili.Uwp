// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.Models.App.Constants
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

        public const double AppMinWidth = 500d;
        public const double AppMinHeight = 500d;

        public const string DashVideoMPDFile = "ms-appx:///Assets/DashVideoTemplate.xml";
        public const string DashVideoWithoudAudioMPDFile = "ms-appx:///Assets/DashVideoWithoutAudioTemplate.xml";

        public const string ImageClickEvent = "ImageClick";
        public const string LinkClickEvent = "LinkClick";

        public const string ReplySection = "Reply";
        public const string ViewLaterSection = "ViewLater";

        public const string LastOpenVideoFileName = "LastOpenVideo.json";
        public const string FixedPublisherFolderName = "Fixed publisher";
        public const string FixedPgcFolderName = "Fixed pgc";
        public const string FixedContentFileName = "User-{0}.json";

        public const string ScreenshotFolderName = "Bili Screenshots";

        /// <summary>
        /// 哔哩哔哩番剧出差账户Id.
        /// </summary>
        public const int RegionalAnimeUserId = 11783021;
#pragma warning restore SA1600 // Elements should be documented
    }
}
