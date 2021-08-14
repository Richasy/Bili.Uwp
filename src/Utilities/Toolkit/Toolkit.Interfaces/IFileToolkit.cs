// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;

namespace Richasy.Bili.Toolkit.Interfaces
{
    /// <summary>
    /// File, IO realated toolkit.
    /// </summary>
    public interface IFileToolkit
    {
        /// <summary>
        /// Get local data and convert.
        /// </summary>
        /// <typeparam name="T">Conversion target type.</typeparam>
        /// <param name="fileName">File name.</param>
        /// <param name="defaultValue">The default value when the file does not exist or has no content.</param>
        /// <param name="folderName">The folder to which the file belongs.</param>
        /// <returns>Converted result.</returns>
        Task<T> ReadLocalDataAsync<T>(string fileName, string defaultValue = "{}", string folderName = "");

        /// <summary>
        /// Write data to local file.
        /// </summary>
        /// <typeparam name="T">Type of data.</typeparam>
        /// <param name="fileName">File name.</param>
        /// <param name="data">Data to be written.</param>
        /// <param name="folderName">The folder to which the file belongs.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task WriteLocalDataAsync<T>(string fileName, T data, string folderName = "");

        /// <summary>
        /// Delete local data file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="folderName">The folder to which the file belongs.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task DeleteLocalDataAsync(string fileName, string folderName = "");

        /// <summary>
        /// Open the file chooser and read the selected file.
        /// </summary>
        /// <param name="types">Allowed file extension.</param>
        /// <returns><c>Item1</c> represents the content of the file, <c>Item2</c> means file extension name.</returns>
        Task<Tuple<string, string>> OpenLocalFileAndReadAsync(params string[] types);

        /// <summary>
        /// Open file which in the package.
        /// </summary>
        /// <param name="filePath">Start with ms-appx:///.</param>
        /// <returns>Text.</returns>
        Task<string> ReadPackageFile(string filePath);
    }
}
