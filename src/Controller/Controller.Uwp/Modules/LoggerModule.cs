﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Richasy.Bili.Controller.Uwp.Interfaces;
using Richasy.Bili.Models.App.Constants;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System.Profile;
using Windows.System.UserProfile;

namespace Richasy.Bili.Controller.Uwp.Modules
{
    /// <summary>
    /// 应用日志记录模块.
    /// </summary>
    public class LoggerModule : ILoggerModule
    {
        private PackageVersion _appVersion;
        private PackageVersion _systemVersion;
        private CultureInfo _culture;
        private string _deviceFamily;
        private string _architecture;

        private NLog.Logger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggerModule"/> class.
        /// </summary>
        public LoggerModule()
        {
            Initialize();
        }

        /// <inheritdoc/>
        public void LogInformation(string message)
        {
            var header = GetHeader();
            _logger.Log(NLog.LogLevel.Info, header + message + "\n");
        }

        /// <inheritdoc/>
        public void LogError(Exception ex, bool isWarning = false)
        {
            var header = GetHeader();
            if (isWarning)
            {
                _logger.Log(NLog.LogLevel.Warn, ex, header + ex.Message);
            }
            else
            {
                _logger.Log(NLog.LogLevel.Error, ex, header);
            }
        }

        private void Initialize()
        {
            _appVersion = Package.Current.Id.Version;
            _culture = GlobalizationPreferences.Languages.Count > 0 ? new CultureInfo(GlobalizationPreferences.Languages.First()) : null;
            _deviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;

            var sysVersion = ulong.Parse(AnalyticsInfo.VersionInfo.DeviceFamilyVersion);
            _systemVersion = new PackageVersion
            {
                Major = (ushort)((sysVersion & 0xFFFF000000000000L) >> 48),
                Minor = (ushort)((sysVersion & 0x0000FFFF00000000L) >> 32),
                Build = (ushort)((sysVersion & 0x00000000FFFF0000L) >> 16),
                Revision = (ushort)(sysVersion & 0x000000000000FFFFL),
            };

            _architecture = Package.Current.Id.Architecture.ToString();

            var rootFolder = ApplicationData.Current.LocalFolder;
            var logFolderName = ControllerConstants.Names.LoggerFolder;
            var fullPath = $"{rootFolder.Path}\\{logFolderName}\\";
            NLog.GlobalDiagnosticsContext.Set("LogPath", fullPath);
            _logger = NLog.LogManager.GetLogger("Richasy.Bili");
        }

        private string GetHeader()
        {
            var sysVersion = $"{_systemVersion.Major}.{_systemVersion.Minor}.{_systemVersion.Build}.{_systemVersion.Revision}";
            var appVersion = $"{_appVersion.Major}.{_appVersion.Minor}.{_appVersion.Build}.{_appVersion.Revision}";
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.AppendLine($"系统信息：{sysVersion} | {_deviceFamily} | {_architecture}");
            builder.AppendLine($"应用信息：{appVersion} | {_culture.DisplayName}");
            builder.AppendLine("--------");

            return builder.ToString();
        }
    }
}
