// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Websocket.Client;

using static Richasy.Bili.Models.App.Constants.ControllerConstants.Live;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器中处理直播的模块.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 请求直播源列表.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestLiveFeedsAsync(int pageNumber)
        {
            try
            {
                ThrowWhenNetworkUnavaliable();
                var data = await _liveProvider.GetLiveFeedsAsync(pageNumber);
                if (pageNumber == 1)
                {
                    var additionalArgs = LiveFeedAdditionalDataChangedEventArgs.Create(data);
                    if (additionalArgs != null)
                    {
                        LiveFeedAdditionalDataChanged?.Invoke(this, additionalArgs);
                    }
                }

                LiveFeedRoomIteration?.Invoke(this, new LiveFeedRoomIterationEventArgs(data, pageNumber + 1));
            }
            catch (ServiceException)
            {
                if (pageNumber == 1)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取直播间详情.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <returns><see cref="LiveRoomDetail"/>.</returns>
        public async Task<LiveRoomDetail> GetLiveRoomDetailAsync(int roomId)
        {
            return await _liveProvider.GetLiveRoomDetailAsync(roomId);
        }

        /// <summary>
        /// 获取直播间播放信息.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <param name="quality">清晰度.</param>
        /// <returns>播放信息.</returns>
        public async Task<LivePlayInformation> GetLivePlayInformationAsync(int roomId, int quality = 4)
        {
            var result = await _liveProvider.GetLivePlayInformationAsync(roomId, quality);
            return result;
        }

        /// <summary>
        /// 连接到直播间.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ConnectToLiveRoomAsync(int roomId)
        {
            await _liveProvider.EnterLiveRoomAsync(roomId);
            ConnectLiveSocket();
            if (_liveConnectionTask != null)
            {
                await _liveConnectionTask.ContinueWith(async result =>
                {
                    if (result.IsCompletedSuccessfully)
                    {
                        await SendLiveMessageAsync(
                            new
                            {
                                roomid = roomId,
                                uid = _accountProvider.UserId,
                            },
                            7);
                    }
                });
            }
        }

        /// <summary>
        /// 发送直播间心跳包.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task SendLiveHeartBeatAsync()
        {
            if (_isLiveSocketConnected)
            {
                await SendLiveMessageAsync(string.Empty, 2);
            }
        }

        /// <summary>
        /// 发送直播弹幕.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <param name="text">弹幕文本.</param>
        /// <returns>发送结果.</returns>
        public async Task<bool> SendLiveDanmakuAsync(int roomId, string text)
        {
            return await _liveProvider.SendMessageAsync(roomId, text);
        }

        private void InitializeLiveSocket()
        {
            CleanupLiveSocket();

            if (_liveWebSocket == null)
            {
                _liveWebSocket = new WebsocketClient(new Uri(ServiceConstants.Api.Live.ChatSocket));
                _liveWebSocket.ErrorReconnectTimeout = TimeSpan.FromSeconds(30);
                _liveWebSocket.DisconnectionHappened.Subscribe(info => OnLiveSocketDisconnected(info));
                _liveWebSocket.MessageReceived.Subscribe(msg => OnLiveSocketMessageReceived(msg));
            }
        }

        private void CleanupLiveSocket()
        {
            if (_liveCancellationToken != null)
            {
                _liveCancellationToken.Cancel();
            }

            _liveCancellationToken = new CancellationTokenSource();

            _isLiveSocketConnected = false;
            _liveWebSocket?.Stop(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, string.Empty);
        }

        private void ConnectLiveSocket()
        {
            if (_isLiveSocketConnected)
            {
                return;
            }

            if (_liveWebSocket == null || (_liveConnectionTask != null && !_liveConnectionTask.IsCompleted))
            {
                this.InitializeLiveSocket();
            }

            this._liveConnectionTask = _liveWebSocket.Start();
            this._liveConnectionTask.ContinueWith((result) =>
            {
                try
                {
                    if (result.IsCompletedSuccessfully)
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
                    // Log.
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
                LiveMessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.ConnectSuccess, "弹幕连接成功"));
            }
            else if (operation == 3)
            {
                var online = BitConverter.ToInt32(body.Reverse().ToArray(), 0);
                LiveMessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.Online, online));
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
                if (cmd.Contains(DanmakuMessage))
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

                        LiveMessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.Danmaku, msg));
                        return;
                    }
                }
                else if (cmd == SendGiftMessage)
                {
                    var msg = new LiveGiftMessage();
                    if (obj["data"] != null)
                    {
                        msg.UserName = obj["data"]["uname"].ToString();
                        msg.Action = obj["data"]["action"].ToString();
                        msg.GiftId = Convert.ToInt32(obj["data"]["giftId"].ToString());
                        msg.GiftName = obj["data"]["giftName"].ToString();
                        msg.TotalNumber = obj["data"]["num"].ToString();
                        msg.UserId = obj["data"]["uid"].ToString();

                        LiveMessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.Gift, msg));
                    }
                }
                else if (cmd == ComboSendGiftMessage)
                {
                    if (obj["data"] != null)
                    {
                        var data = obj["data"].ToString();
                        var msg = JsonConvert.DeserializeObject<LiveGiftMessage>(data);
                        LiveMessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.Gift, msg));
                    }
                }
                else if (cmd == WelcomeMessage)
                {
                    if (obj["data"] != null)
                    {
                        var data = obj["data"].ToString();
                        var msg = JsonConvert.DeserializeObject<LiveWelcomeMessage>(data);
                        LiveMessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.Welcome, msg));
                    }
                }
                else if (cmd == SystemMessage)
                {
                    LiveMessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.SystemMsg, obj["msg"].ToString()));
                }
                else if (cmd == AnchorLotteryStartMessage)
                {
                    if (obj["data"] != null)
                    {
                        LiveMessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.AnchorLotteryStart, obj["data"].ToString()));
                    }
                }
                else if (cmd == AnchorLotteryAwardMessage)
                {
                    if (obj["data"] != null)
                    {
                        LiveMessageReceived?.Invoke(this, new LiveMessageEventArgs(LiveMessageType.AnchorLotteryAward, obj["data"].ToString()));
                    }
                }
                else if (cmd == SuperChatMessage)
                {
                    // 暂不支持.
                }
            }
            catch (Exception)
            {
                // 记录日志.
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
