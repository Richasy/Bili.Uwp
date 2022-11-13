// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;
using Bili.Toolkit.Interfaces;
using Newtonsoft.Json;
using Windows.Storage;

namespace Bili.Toolkit.Uwp
{
    /// <summary>
    /// File Toolkit.
    /// </summary>
    public class FileToolkit : IFileToolkit
    {
        /// <inheritdoc/>
        public Task<T> ReadLocalDataAsync<T>(string fileName, string defaultValue = "{}", string folderName = "") => Task.Run(async () =>
        {
            var path = string.IsNullOrEmpty(folderName) ?
                            $"ms-appdata:///local/{fileName}" :
                            $"ms-appdata:///local/{folderName}/{fileName}";
            var content = defaultValue;
            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path))
                        .AsTask();
                var fileContent = await FileIO.ReadTextAsync(file)
                               .AsTask();

                if (!string.IsNullOrEmpty(fileContent))
                {
                    content = fileContent;
                }
            }
            catch (FileNotFoundException)
            {
            }

            if (typeof(T) == typeof(string))
            {
                return (T)content.Clone();
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
        });

        /// <inheritdoc/>
        public Task WriteLocalDataAsync<T>(string fileName, T data, string folderName = "") => Task.Run(async () =>
        {
            var folder = ApplicationData.Current.LocalFolder;

            if (!string.IsNullOrEmpty(folderName))
            {
                folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists)
                            .AsTask();
            }

            var writeContent = string.Empty;
            writeContent = data is string
                ? data.ToString()
                : JsonConvert.SerializeObject(data);

            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists)
                        .AsTask();

            await FileIO.WriteTextAsync(file, writeContent)
              .AsTask();
        });

        /// <inheritdoc/>
        public Task DeleteLocalDataAsync(string fileName, string folderName = "") => Task.Run(async () =>
        {
            var folder = ApplicationData.Current.LocalFolder;

            if (!string.IsNullOrEmpty(folderName))
            {
                folder = await folder.CreateFolderAsync(folderName)
                            .AsTask();
            }

            var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists)
                        .AsTask();
            await file.DeleteAsync()
                .AsTask();
        });

        /// <inheritdoc/>
        public async Task<string> ReadPackageFile(string filePath)
        {
            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(filePath));
                return await FileIO.ReadTextAsync(file);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
