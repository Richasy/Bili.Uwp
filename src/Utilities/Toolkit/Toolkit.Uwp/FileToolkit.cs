// Copyright (c) Richasy. All rights reserved.

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Richasy.Bili.Toolkit.Uwp
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
            if (data is string)
            {
                writeContent = data.ToString();
            }
            else
            {
                writeContent = JsonConvert.SerializeObject(data);
            }

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
        public async Task<Tuple<string, string>> OpenLocalFileAndReadAsync(params string[] types)
        {
            var picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            var typeReg = new Regex(@"^\.[a-zA-Z0-9]+$");
            foreach (var type in types)
            {
                if (type == "*" || typeReg.IsMatch(type))
                {
                    picker.FileTypeFilter.Add(type);
                }
                else
                {
                    throw new InvalidCastException("Invalid file extension.");
                }
            }

            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                var content = await FileIO.ReadTextAsync(file);
                return new Tuple<string, string>(content, file.FileType);
            }

            return null;
        }
    }
}
