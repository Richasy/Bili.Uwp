// Copyright (c) Richasy. All rights reserved.

#pragma warning disable SA1300 // Element should begin with upper-case letter
using System.Security.Cryptography;
using System.Text;

namespace Bili.Models.gRPC
{
    /// <summary>
    /// BUVID.
    /// </summary>
    public class Buvid
    {
        private readonly string _mac;

        /// <summary>
        /// Initializes a new instance of the <see cref="Buvid"/> class.
        /// </summary>
        /// <param name="macAddress">MAC 地址.</param>
        public Buvid(string macAddress) => _mac = macAddress;

        /// <summary>
        /// 生成 buvid.
        /// </summary>
        /// <returns>buvid.</returns>
        public string Generate()
        {
            var buvidPrefix = "XY";
            var inputStrMd5 = GetMd5Hash(_mac.Replace(":", string.Empty));

            var buvidRaw = new StringBuilder();
            buvidRaw.Append(buvidPrefix);
            buvidRaw.Append(inputStrMd5[2]);
            buvidRaw.Append(inputStrMd5[12]);
            buvidRaw.Append(inputStrMd5[22]);
            buvidRaw.Append(inputStrMd5);

            return buvidRaw.ToString();
        }

        private static string GetMd5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(input);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
