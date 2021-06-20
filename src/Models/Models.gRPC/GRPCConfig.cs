// Copyright (c) Richasy. All rights reserved.

#pragma warning disable SA1300 // Element should begin with upper-case letter
using System;
using Bilibili.Metadata;
using Bilibili.Metadata.Device;
using Bilibili.Metadata.Fawkes;
using Bilibili.Metadata.Locale;
using Bilibili.Metadata.Network;
using Bilibili.Metadata.Restriction;
using Google.Protobuf;

namespace Richasy.Bili.Models.gRPC
{
    /// <summary>
    /// gRPC的请求配置.
    /// </summary>
    public class GRPCConfig
    {
        /// <summary>
        /// 系统版本.
        /// </summary>
        public const string OSVersion = "14.6";

        /// <summary>
        /// 厂商.
        /// </summary>
        public const string Brand = "Apple";

        /// <summary>
        /// 手机系统.
        /// </summary>
        public const string Model = "iPhone 11";

        /// <summary>
        /// 应用版本.
        /// </summary>
        public const string AppVersion = "6.7.0";

        /// <summary>
        /// 构建标识.
        /// </summary>
        public const int Build = 6070600;

        /// <summary>
        /// 频道.
        /// </summary>
        public const string Channel = "bilibili140";

        /// <summary>
        /// 网络状况.
        /// </summary>
        public const int NetworkType = 2;

        /// <summary>
        /// 未知.
        /// </summary>
        public const int NetworkTF = 0;

        /// <summary>
        /// 未知.
        /// </summary>
        public const string NetworkOid = "46007";

        /// <summary>
        /// 未知.
        /// </summary>
        public const string Cronet = "1.21.0";

        /// <summary>
        /// 未知.
        /// </summary>
        public const string Buvid = "XZFD48CFF1E68E637D0DF11A562468A8DC314";

        /// <summary>
        /// 应用类型.
        /// </summary>
        public const string MobileApp = "iphone";

        /// <summary>
        /// 移动平台.
        /// </summary>
        public const string Platform = "iphone";

        /// <summary>
        /// 产品环境.
        /// </summary>
        public const string Envorienment = "prod";

        /// <summary>
        /// 应用Id.
        /// </summary>
        public const int AppId = 1;

        /// <summary>
        /// 国家或地区.
        /// </summary>
        public const string Region = "CN";

        /// <summary>
        /// 语言.
        /// </summary>
        public const string Language = "zh";

        /// <summary>
        /// Initializes a new instance of the <see cref="GRPCConfig"/> class.
        /// </summary>
        /// <param name="accessToken">访问令牌.</param>
        public GRPCConfig(string accessToken)
        {
            this.AccessToken = accessToken;
        }

        /// <summary>
        /// 访问令牌.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 获取客户端在Fawkes系统中的信息标头.
        /// </summary>
        /// <returns>Base64字符串.</returns>
        public string GetFawkesreqBin()
        {
            var msg = new FawkesReq();
            msg.Appkey = MobileApp;
            msg.Env = Envorienment;
            return ToBase64(msg.ToByteArray());
        }

        /// <summary>
        /// 获取元数据标头.
        /// </summary>
        /// <returns>Base64字符串.</returns>
        public string GetMetadataBin()
        {
            var msg = new Metadata();
            msg.AccessKey = AccessToken;
            msg.MobiApp = MobileApp;
            msg.Build = Build;
            msg.Channel = Channel;
            msg.Buvid = Buvid;
            msg.Platform = Platform;
            return ToBase64(msg.ToByteArray());
        }

        /// <summary>
        /// 获取设备标头.
        /// </summary>
        /// <returns>Base64字符串.</returns>
        public string GetDeviceBin()
        {
            var msg = new Device();
            msg.AppId = AppId;
            msg.MobiApp = MobileApp;
            msg.Build = Build;
            msg.Channel = Channel;
            msg.Buvid = Buvid;
            msg.Platform = Platform;
            msg.Brand = Brand;
            msg.Model = Model;
            msg.Osver = OSVersion;
            return ToBase64(msg.ToByteArray());
        }

        /// <summary>
        /// 获取网络标头.
        /// </summary>
        /// <returns>Base64字符串.</returns>
        public string GetNetworkBin()
        {
            var msg = new Network();
            msg.Type = Bilibili.Metadata.Network.NetworkType.Wifi;
            msg.Oid = NetworkOid;
            return ToBase64(msg.ToByteArray());
        }

        /// <summary>
        /// 获取限制标头.
        /// </summary>
        /// <returns>Base64字符串.</returns>
        public string GetRestrictionBin()
        {
            var msg = new Restriction();

            return ToBase64(msg.ToByteArray());
        }

        /// <summary>
        /// 获取本地化标头.
        /// </summary>
        /// <returns>Base64字符串.</returns>
        public string GetLocaleBin()
        {
            var msg = new Locale();
            msg.CLocale = new LocaleIds();
            msg.SLocale = new LocaleIds();
            msg.CLocale.Language = Language;
            msg.CLocale.Region = Region;
            msg.SLocale.Language = Language;
            msg.SLocale.Region = Region;
            return ToBase64(msg.ToByteArray());
        }

        /// <summary>
        /// 将数据转换为Base64字符串.
        /// </summary>
        /// <param name="data">数据.</param>
        /// <returns>Base64字符串.</returns>
        public string ToBase64(byte[] data)
        {
            return Convert.ToBase64String(data).TrimEnd('=');
        }
    }
}
#pragma warning restore SA1300 // Element should begin with upper-case letter
