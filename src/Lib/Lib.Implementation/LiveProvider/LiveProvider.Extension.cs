// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Live;
using Bili.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Bili.Lib
{
    /// <summary>
    /// 提供直播相关的操作.
    /// </summary>
    public partial class LiveProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly ILiveAdapter _liveAdapter;
        private readonly ICommunityAdapter _communityAdapter;

        /// <summary>
        /// 直播间套接字.
        /// </summary>
        private WebsocketClient _liveWebSocket;
        private CancellationTokenSource _liveCancellationToken;
        private bool _isLiveSocketConnected;
        private Task _liveConnectionTask;

        private int _feedPageNumber;
        private int _partitionPageNumber;

        private void InitializeLiveSocket()
        {
            ResetLiveConnection();

            if (_liveWebSocket == null)
            {
                _liveWebSocket = new WebsocketClient(new Uri(ApiConstants.Live.ChatSocket));
                _liveWebSocket.ErrorReconnectTimeout = TimeSpan.FromSeconds(30);
                _liveWebSocket.DisconnectionHappened.Subscribe(info => OnLiveSocketDisconnected(info));
                _liveWebSocket.MessageReceived.Subscribe(msg => OnLiveSocketMessageReceived(msg));
            }
        }

        private void ConnectLiveSocket()
        {
            if (_isLiveSocketConnected)
            {
                return;
            }

            if (_liveWebSocket == null || (_liveConnectionTask != null && !_liveConnectionTask.IsCompleted))
            {
                InitializeLiveSocket();
            }

            _liveConnectionTask = _liveWebSocket.Start();
            _liveConnectionTask.ContinueWith((result) =>
            {
                try
                {
                    if (result.IsCompleted)
                    {
                        _isLiveSocketConnected = true;
                    }
                    else if (result.IsFaulted)
                    {
                        throw result.Exception;
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private async Task SendLiveMessageAsync(object data, int action)
        {
            var messageText = data is string str ? str : JsonConvert.SerializeObject(data);
            var messageData = EncodeLiveData(messageText, action);
            await _liveWebSocket.SendInstant(messageData);
        }

        /// <summary>
        /// 对数据进行编码.
        /// </summary>
        /// <param name="msg">文本内容.</param>
        /// <param name="action">2=心跳，7=进房.</param>
        /// <returns>编码后的数据.</returns>
        private byte[] EncodeLiveData(string msg, int action)
        {
            var data = Encoding.UTF8.GetBytes(msg);

            // 头部长度固定16
            var length = data.Length + 16;
            var buffer = new byte[length];
            using (var ms = new MemoryStream(buffer))
            {
                // 数据包长度
                var b = BitConverter.GetBytes(buffer.Length).ToArray().Reverse().ToArray();
                ms.Write(b, 0, 4);

                // 数据包头部长度,固定16
                b = BitConverter.GetBytes(16).Reverse().ToArray();
                ms.Write(b, 2, 2);

                // 协议版本，0=JSON,1=Int32,2=Buffer
                b = BitConverter.GetBytes(0).Reverse().ToArray();
                ms.Write(b, 0, 2);

                // 操作类型
                b = BitConverter.GetBytes(action).Reverse().ToArray();
                ms.Write(b, 0, 4);

                // 数据包头部长度,固定1
                b = BitConverter.GetBytes(1).Reverse().ToArray();
                ms.Write(b, 0, 4);

                // 数据
                ms.Write(data, 0, data.Length);

                var bytes = ms.ToArray();
                ms.Flush();
                return bytes;
            }
        }

        /// <summary>
        /// 解压直播数据.
        /// </summary>
        /// <param name="data">数据.</param>
        /// <returns>解压后的数据.</returns>
        private byte[] DecompressData(byte[] data)
        {
            using (var outBuffer = new MemoryStream())
            {
                using (var compressedzipStream = new DeflateStream(new MemoryStream(data, 2, data.Length - 2), CompressionMode.Decompress))
                {
                    var block = new byte[1024];
                    while (true)
                    {
                        var bytesRead = compressedzipStream.Read(block, 0, block.Length);
                        if (bytesRead <= 0)
                        {
                            break;
                        }
                        else
                        {
                            outBuffer.Write(block, 0, bytesRead);
                        }
                    }

                    compressedzipStream.Close();
                    return outBuffer.ToArray();
                }
            }
        }

        private void ParseLiveData(byte[] data)
        {
            // 协议版本。
            // 0为JSON，可以直接解析；
            // 1为房间人气值,Body为Int32；
            // 2为压缩过Buffer，需要解压再处理
            var protocolVersion = BitConverter.ToInt32(new byte[4] { data[7], data[6], 0, 0 }, 0);

            // 操作类型。
            // 3=心跳回应，内容为房间人气值；
            // 5=通知，弹幕、广播等全部信息；
            // 8=进房回应，空
            var operation = BitConverter.ToInt32(data.Skip(8).Take(4).Reverse().ToArray(), 0);

            // 内容
            var body = data.Skip(16).ToArray();
            if (operation == 8)
            {
                MessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.ConnectSuccess, "弹幕连接成功"));
            }
            else if (operation == 3)
            {
                var online = BitConverter.ToInt32(body.Reverse().ToArray(), 0);
                MessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.Online, online));
            }
            else if (operation == 5)
            {
                if (protocolVersion == 2)
                {
                    body = DecompressData(body);
                }

                var text = Encoding.UTF8.GetString(body);

                // 可能有多条数据，做个分割
                var textLines = Regex.Split(text, "[\x00-\x1f]+").Where(x => x.Length > 2 && x[0] == '{').ToArray();
                foreach (var item in textLines)
                {
                    ParseMessage(item);
                }
            }
        }

        private void ParseMessage(string jsonMessage)
        {
            try
            {
                var obj = JObject.Parse(jsonMessage);
                var cmd = obj["cmd"].ToString();
                if (cmd.Contains("DANMU_MSG"))
                {
                    var msg = new LiveDanmakuMessage();
                    if (obj["info"] != null && obj["info"].ToArray().Length != 0)
                    {
                        msg.Text = obj["info"][1].ToString();
                        if (obj["info"][2] != null && obj["info"][2].ToArray().Length != 0)
                        {
                            msg.UserName = obj["info"][2][1].ToString() + ":";

                            if (obj["info"][2][3] != null && Convert.ToInt32(obj["info"][2][3].ToString()) == 1)
                            {
                                msg.VipText = "老爷";
                                msg.IsVip = true;
                            }

                            if (obj["info"][2][4] != null && Convert.ToInt32(obj["info"][2][4].ToString()) == 1)
                            {
                                msg.VipText = "年费老爷";
                                msg.IsVip = false;
                                msg.IsBigVip = true;
                            }

                            if (obj["info"][2][2] != null && Convert.ToInt32(obj["info"][2][2].ToString()) == 1)
                            {
                                msg.VipText = "房管";
                                msg.IsAdmin = true;
                            }
                        }

                        if (obj["info"][3] != null && obj["info"][3].ToArray().Length != 0)
                        {
                            msg.MedalName = obj["info"][3][1].ToString();
                            msg.MedalLevel = obj["info"][3][0].ToString();
                            msg.MedalColor = obj["info"][3][4].ToString();
                            msg.HasMedal = true;
                        }

                        if (obj["info"][4] != null && obj["info"][4].ToArray().Length != 0)
                        {
                            msg.Level = "UL" + obj["info"][4][0].ToString();
                            msg.LevelColor = obj["info"][4][2].ToString();
                        }

                        if (obj["info"][5] != null && obj["info"][5].ToArray().Length != 0)
                        {
                            msg.UserTitle = obj["info"][5][0].ToString();
                            msg.HasTitle = true;
                        }

                        var info = new LiveDanmakuInformation(msg.Text, msg.UserName, msg.Level, msg.LevelColor, msg.IsAdmin);
                        MessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.Danmaku, info));
                    }
                }
            }
            catch (Exception)
            {
                // TODO: 记录错误.
            }
        }

        private void OnLiveSocketMessageReceived(ResponseMessage msg)
        {
            try
            {
                switch (msg.MessageType)
                {
                    case System.Net.WebSockets.WebSocketMessageType.Binary:
                        ParseLiveData(msg.Binary);
                        break;
                    case System.Net.WebSockets.WebSocketMessageType.Close:
                        if (_isLiveSocketConnected)
                        {
                            InitializeLiveSocket();
                            ConnectLiveSocket();
                        }

                        break;
                    case System.Net.WebSockets.WebSocketMessageType.Text:
                        ParseMessage(msg.Text);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                InitializeLiveSocket();
                ConnectLiveSocket();
            }
        }

        private void OnLiveSocketDisconnected(DisconnectionInfo info)
        {
            if (_isLiveSocketConnected)
            {
                _isLiveSocketConnected = false;
                _liveWebSocket?.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, string.Empty);
                ConnectLiveSocket();
            }
        }
    }
}
