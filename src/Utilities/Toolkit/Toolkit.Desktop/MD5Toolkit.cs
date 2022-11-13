// Copyright (c) Richasy. All rights reserved.

using System;
using System.Text;
using Bili.Toolkit.Interfaces;

namespace Bili.Toolkit.Desktop
{
    /// <summary>
    /// MD5工具.
    /// </summary>
    public class MD5Toolkit : IMD5Toolkit
    {
#pragma warning disable CA1822 // Mark members as static
        private readonly byte[] _buffer = new byte[0x40];
        private readonly uint[] _count = new uint[2];
        private readonly uint[] _state = new uint[4];
        private readonly byte[] _padding;
        private byte[] _hashValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="MD5Toolkit"/> class.
        /// </summary>
        public MD5Toolkit()
        {
            var buffer = new byte[0x40];
            buffer[0] = 0x80;
            _padding = buffer;
            Initialize();
        }

        /// <inheritdoc/>
        public string GetMd5String(string source)
        {
            var bytes = new UTF8Encoding().GetBytes(source);
            var buffer = ComputeHash(bytes);
            var builder = new StringBuilder();
            foreach (var num in buffer)
            {
                builder.Append(num.ToString("x2"));
            }

            return builder.ToString();
        }

        /// <inheritdoc/>
        public void Dispose()
            => Dispose(true);

        private void Initialize()
        {
            _count[0] = _count[1] = 0;
            _state[0] = 0x67452301;
            _state[1] = 0xefcdab89;
            _state[2] = 0x98badcfe;
            _state[3] = 0x10325476;
        }

        private byte[] ComputeHash(byte[] buffer)
            => ComputeHash(buffer, 0, buffer.Length);

        private byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            Initialize();
            HashCore(buffer, offset, count);
            _hashValue = HashFinal();
            return (byte[])_hashValue.Clone();
        }

        private void Decode(uint[] output, int outputOffset, byte[] input, int inputOffset, int count)
        {
            var num3 = inputOffset + count;
            var index = outputOffset;
            for (var i = inputOffset; i < num3; i += 4)
            {
                output[index] = (uint)(input[i] | (input[i + 1] << 8)) | (uint)(input[i + 2] << 0x10) | (uint)(input[i + 3] << 0x18);
                index++;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
            {
                Initialize();
            }
        }

        private void Encode(byte[] output, int outputOffset, uint[] input, int inputOffset, int count)
        {
            var num3 = outputOffset + count;
            var index = inputOffset;
            for (var i = outputOffset; i < num3; i += 4)
            {
                output[i] = (byte)(input[index] & 0xff);
                output[i + 1] = (byte)((input[index] >> 8) & 0xff);
                output[i + 2] = (byte)((input[index] >> 0x10) & 0xff);
                output[i + 3] = (byte)((input[index] >> 0x18) & 0xff);
                index++;
            }
        }

        private uint F(uint x, uint y, uint z)
            => (x & y) | (~x & z);

        private void FF(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
        {
            a += F(b, c, d) + x + ac;
            a = ROTATE_LEFT(a, s);
            a += b;
        }

        private uint G(uint x, uint y, uint z)
            => (x & z) | (y & ~z);

        private void GG(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
        {
            a += G(b, c, d) + x + ac;
            a = ROTATE_LEFT(a, s);
            a += b;
        }

        private uint H(uint x, uint y, uint z)
            => x ^ y ^ z;

        private void HashCore(byte[] input, int offset, int count)
        {
            var num = 0;
            var num2 = (int)((_count[0] >> 3) & 0x3f);
            if ((_count[0] += (uint)(count << 3)) < (count << 3))
            {
                _count[1]++;
            }

            _count[1] += (uint)count >> 0x1d;
            var num3 = 0x40 - num2;
            if (count >= num3)
            {
                Buffer.BlockCopy(input, offset, _buffer, num2, num3);
                Transform(_buffer, 0);
                for (num = num3; (num + 0x3f) < count; num += 0x40)
                {
                    Transform(input, offset + num);
                }

                num2 = 0;
            }
            else
            {
                num = 0;
            }

            Buffer.BlockCopy(input, offset + num, _buffer, num2, count - num);
        }

        private byte[] HashFinal()
        {
            var output = new byte[0x10];
            var buffer2 = new byte[8];
            Encode(buffer2, 0, _count, 0, 8);
            var num = (int)((_count[0] >> 3) & 0x3f);
            var count = (num < 0x38) ? (0x38 - num) : (120 - num);
            HashCore(_padding, 0, count);
            HashCore(buffer2, 0, 8);
            Encode(output, 0, _state, 0, 0x10);
            _count[0] = _count[1] = 0;
            _state[0] = 0;
            _state[1] = 0;
            _state[2] = 0;
            _state[3] = 0;
            Initialize();
            return output;
        }

        private void HH(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
        {
            a += H(b, c, d) + x + ac;
            a = ROTATE_LEFT(a, s);
            a += b;
        }

        private uint I(uint x, uint y, uint z) => y ^ (x | ~z);

        private void II(ref uint a, uint b, uint c, uint d, uint x, byte s, uint ac)
        {
            a += I(b, c, d) + x + ac;
            a = ROTATE_LEFT(a, s);
            a += b;
        }

        private uint ROTATE_LEFT(uint x, byte n) => (x << n) | (x >> (0x20 - n));

        private void Transform(byte[] block, int offset)
        {
            var a = _state[0];
            var b = _state[1];
            var c = _state[2];
            var d = _state[3];
            var output = new uint[0x10];
            Decode(output, 0, block, offset, 0x40);
            FF(ref a, b, c, d, output[0], 7, 0xd76aa478);
            FF(ref d, a, b, c, output[1], 12, 0xe8c7b756);
            FF(ref c, d, a, b, output[2], 0x11, 0x242070db);
            FF(ref b, c, d, a, output[3], 0x16, 0xc1bdceee);
            FF(ref a, b, c, d, output[4], 7, 0xf57c0faf);
            FF(ref d, a, b, c, output[5], 12, 0x4787c62a);
            FF(ref c, d, a, b, output[6], 0x11, 0xa8304613);
            FF(ref b, c, d, a, output[7], 0x16, 0xfd469501);
            FF(ref a, b, c, d, output[8], 7, 0x698098d8);
            FF(ref d, a, b, c, output[9], 12, 0x8b44f7af);
            FF(ref c, d, a, b, output[10], 0x11, 0xffff5bb1);
            FF(ref b, c, d, a, output[11], 0x16, 0x895cd7be);
            FF(ref a, b, c, d, output[12], 7, 0x6b901122);
            FF(ref d, a, b, c, output[13], 12, 0xfd987193);
            FF(ref c, d, a, b, output[14], 0x11, 0xa679438e);
            FF(ref b, c, d, a, output[15], 0x16, 0x49b40821);
            GG(ref a, b, c, d, output[1], 5, 0xf61e2562);
            GG(ref d, a, b, c, output[6], 9, 0xc040b340);
            GG(ref c, d, a, b, output[11], 14, 0x265e5a51);
            GG(ref b, c, d, a, output[0], 20, 0xe9b6c7aa);
            GG(ref a, b, c, d, output[5], 5, 0xd62f105d);
            GG(ref d, a, b, c, output[10], 9, 0x2441453);
            GG(ref c, d, a, b, output[15], 14, 0xd8a1e681);
            GG(ref b, c, d, a, output[4], 20, 0xe7d3fbc8);
            GG(ref a, b, c, d, output[9], 5, 0x21e1cde6);
            GG(ref d, a, b, c, output[14], 9, 0xc33707d6);
            GG(ref c, d, a, b, output[3], 14, 0xf4d50d87);
            GG(ref b, c, d, a, output[8], 20, 0x455a14ed);
            GG(ref a, b, c, d, output[13], 5, 0xa9e3e905);
            GG(ref d, a, b, c, output[2], 9, 0xfcefa3f8);
            GG(ref c, d, a, b, output[7], 14, 0x676f02d9);
            GG(ref b, c, d, a, output[12], 20, 0x8d2a4c8a);
            HH(ref a, b, c, d, output[5], 4, 0xfffa3942);
            HH(ref d, a, b, c, output[8], 11, 0x8771f681);
            HH(ref c, d, a, b, output[11], 0x10, 0x6d9d6122);
            HH(ref b, c, d, a, output[14], 0x17, 0xfde5380c);
            HH(ref a, b, c, d, output[1], 4, 0xa4beea44);
            HH(ref d, a, b, c, output[4], 11, 0x4bdecfa9);
            HH(ref c, d, a, b, output[7], 0x10, 0xf6bb4b60);
            HH(ref b, c, d, a, output[10], 0x17, 0xbebfbc70);
            HH(ref a, b, c, d, output[13], 4, 0x289b7ec6);
            HH(ref d, a, b, c, output[0], 11, 0xeaa127fa);
            HH(ref c, d, a, b, output[3], 0x10, 0xd4ef3085);
            HH(ref b, c, d, a, output[6], 0x17, 0x4881d05);
            HH(ref a, b, c, d, output[9], 4, 0xd9d4d039);
            HH(ref d, a, b, c, output[12], 11, 0xe6db99e5);
            HH(ref c, d, a, b, output[15], 0x10, 0x1fa27cf8);
            HH(ref b, c, d, a, output[2], 0x17, 0xc4ac5665);
            II(ref a, b, c, d, output[0], 6, 0xf4292244);
            II(ref d, a, b, c, output[7], 10, 0x432aff97);
            II(ref c, d, a, b, output[14], 15, 0xab9423a7);
            II(ref b, c, d, a, output[5], 0x15, 0xfc93a039);
            II(ref a, b, c, d, output[12], 6, 0x655b59c3);
            II(ref d, a, b, c, output[3], 10, 0x8f0ccc92);
            II(ref c, d, a, b, output[10], 15, 0xffeff47d);
            II(ref b, c, d, a, output[1], 0x15, 0x85845dd1);
            II(ref a, b, c, d, output[8], 6, 0x6fa87e4f);
            II(ref d, a, b, c, output[15], 10, 0xfe2ce6e0);
            II(ref c, d, a, b, output[6], 15, 0xa3014314);
            II(ref b, c, d, a, output[13], 0x15, 0x4e0811a1);
            II(ref a, b, c, d, output[4], 6, 0xf7537e82);
            II(ref d, a, b, c, output[11], 10, 0xbd3af235);
            II(ref c, d, a, b, output[2], 15, 0x2ad7d2bb);
            II(ref b, c, d, a, output[9], 0x15, 0xeb86d391);
            _state[0] += a;
            _state[1] += b;
            _state[2] += c;
            _state[3] += d;
            for (var i = 0; i < output.Length; i++)
            {
                output[i] = 0;
            }
        }
    }
}
