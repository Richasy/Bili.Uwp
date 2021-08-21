// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

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
        /// <returns>播放信息.</returns>
        public async Task<LivePlayInformation> GetLivePlayInformationAsync(int roomId)
        {
            var result = await _liveProvider.GetLivePlayInformationAsync(roomId);
            return result;
        }

        /// <summary>
        /// 连接到直播间.
        /// </summary>
        /// <param name="roomId">直播间Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ConnectToLiveRoomAsync(int roomId)
        {
            ConnectLiveSocket();
            await _liveConnectionTask.ContinueWith(async (result) =>
            {
                await SendLiveMessageAsync(
                    new
                    {
                        roomid = roomId,
                        uid = _accountProvider.UserId,
                    },
                    7);
            });
        }

        private void InitializeLiveSocket()
        {
            CleanupLiveSocket();

            _liveWebSocket = new MessageWebSocket();
            _liveWebSocket.Control.MessageType = SocketMessageType.Binary;
            _liveWebSocket.Control.DesiredUnsolicitedPongInterval = TimeSpan.FromSeconds(1);

            _liveWebSocket.MessageReceived += OnLiveSocketMessageReceived;
            _liveWebSocket.Closed += OnLiveSocketMessageClosed;
        }

        private void CleanupLiveSocket()
        {
            _isLiveSocketConnected = false;
            _liveWebSocket?.Close(1000, string.Empty);
            _liveWebSocket?.Dispose();
            _liveWebSocket = null;
        }

        private void ConnectLiveSocket()
        {
            if (_isLiveSocketConnected)
            {
                return;
            }

            if (_liveConnectionTask != null && !_liveConnectionTask.IsCompleted)
            {
                _liveCancellationToken.Cancel();
                _liveCancellationToken = new CancellationTokenSource();
                this.InitializeLiveSocket();
            }

            this._liveConnectionTask = _liveWebSocket.ConnectAsync(new Uri(ServiceConstants.Api.Live.ChatSocket)).AsTask(_liveCancellationToken.Token);
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
            var messageWriter = new DataWriter(this._liveWebSocket.OutputStream);
            var messageText = JsonConvert.SerializeObject(data);
            var messageData = EncodeLiveData(messageText, action);
            messageWriter.WriteBytes(messageData);
            await messageWriter.StoreAsync().AsTask().ContinueWith((result) =>
            {
                messageWriter.DetachStream();
            });
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

        private void OnLiveSocketMessageClosed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            // 如果非用户操作导致失去连接，那么就重连.
            if (_isLiveSocketConnected)
            {
                InitializeLiveSocket();
            }
        }

        private void OnLiveSocketMessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
        }
    }
}
