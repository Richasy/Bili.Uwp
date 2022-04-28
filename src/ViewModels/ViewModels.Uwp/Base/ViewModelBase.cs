// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;
using Windows.Storage;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// ViewModel的基类.
    /// </summary>
    public class ViewModelBase : ReactiveObject
    {
        /// <summary>
        /// 加载模拟数据.
        /// </summary>
        /// <typeparam name="T">需要转换的数据类型.</typeparam>
        /// <param name="requestName">请求的Mock文件名.</param>
        /// <returns><see cref="Task{{T}}"/>.</returns>
        protected static async Task<T> LoadMockDataAsync<T>(string requestName)
        {
            var fileUri = new Uri($"ms-appx:///Assets/Mock/{requestName}.json");
            var file = await StorageFile.GetFileFromApplicationUriAsync(fileUri);
            if (file != null)
            {
                var json = await FileIO.ReadTextAsync(file);
                return JsonConvert.DeserializeObject<T>(json);
            }

            return default;
        }
    }
}
