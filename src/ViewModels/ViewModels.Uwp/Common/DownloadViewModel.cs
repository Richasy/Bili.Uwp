// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Locator.Uwp;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 下载配置视图模型.
    /// </summary>
    public class DownloadViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadViewModel"/> class.
        /// </summary>
        protected DownloadViewModel()
        {
            UseMultiThread = true;
            TotalPartCollection = new ObservableCollection<NumberPartViewModel>();
        }

        /// <summary>
        /// 实例.
        /// </summary>
        public static DownloadViewModel Instance { get; } = new Lazy<DownloadViewModel>(() => new DownloadViewModel()).Value;

        /// <summary>
        /// 下载参数.
        /// </summary>
        public string DownloadParameter { get; set; }

        /// <summary>
        /// 使用MP4Box来混流.
        /// </summary>
        [Reactive]
        public bool UseMp4Box { get; set; }

        /// <summary>
        /// 仅下载HEVC源.
        /// </summary>
        [Reactive]
        public bool OnlyHevc { get; set; }

        /// <summary>
        /// 仅下载AVC源.
        /// </summary>
        [Reactive]
        public bool OnlyAvc { get; set; }

        /// <summary>
        /// 是否仅下载音频.
        /// </summary>
        [Reactive]
        public bool OnlyAudio { get; set; }

        /// <summary>
        /// 是否仅下载视频.
        /// </summary>
        [Reactive]
        public bool OnlyVideo { get; set; }

        /// <summary>
        /// 是否仅下载字幕.
        /// </summary>
        [Reactive]
        public bool OnlySubtitle { get; set; }

        /// <summary>
        /// 使用多线程.
        /// </summary>
        [Reactive]
        public bool UseMultiThread { get; set; }

        /// <summary>
        /// 全部分P集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<NumberPartViewModel> TotalPartCollection { get; set; }

        /// <summary>
        /// 加载.
        /// </summary>
        /// <param name="downloadUrl">下载地址.</param>
        /// <param name="partList">分集列表.</param>
        public void Load(string downloadUrl, List<int> partList)
        {
            DownloadParameter = downloadUrl;
            TotalPartCollection.Clear();
            foreach (var item in partList)
            {
                TotalPartCollection.Add(new NumberPartViewModel(item, true));
            }
        }

        /// <summary>
        /// 创建下载命令.
        /// </summary>
        /// <returns>下载命令.</returns>
        public async Task<string> CreateDownloadCommandAsync()
        {
            var list = new List<string>
            {
                "BBDown",
            };

            if (AccountViewModel.Instance.Status == AccountViewModelStatus.Login)
            {
                var authProvider = ServiceLocator.Instance.GetService<IAuthorizeProvider>();
                var token = await authProvider.GetTokenAsync();
                list.Add("-app");
                list.Add("-token");
                list.Add(token);
            }

            if (UseMp4Box)
            {
                list.Add("--use-mp4box");
            }

            if (OnlyHevc)
            {
                list.Add("-hevc");
            }
            else if (OnlyAvc)
            {
                list.Add("-avc");
            }

            if (UseMultiThread)
            {
                list.Add("-mt");
            }

            var selectedPages = TotalPartCollection.Where(p => p.IsSelected).Select(p => p.Data).ToList();
            if (selectedPages.Count > 0)
            {
                var pageStr = string.Join(',', selectedPages);
                list.Add("-p");
                list.Add(pageStr);
            }

            if (OnlyAudio)
            {
                list.Add("--audio-only");
            }
            else if (OnlyVideo)
            {
                list.Add("--video-only");
            }
            else if (OnlySubtitle)
            {
                list.Add("--sub-only");
            }

            list.Add($"\"{DownloadParameter}\"");

            return string.Join(' ', list);
        }
    }
}
