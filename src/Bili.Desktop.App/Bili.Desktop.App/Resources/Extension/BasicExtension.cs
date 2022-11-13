// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI;
using Microsoft.UI.Xaml;

namespace Bili.Desktop.App.Resources.Extension
{
    /// <summary>
    /// 基础扩展.
    /// </summary>
    public static class BasicExtension
    {
        /// <summary>
        /// 将对象转换为Int32.
        /// </summary>
        /// <param name="obj">对象.</param>
        /// <returns><see cref="int"/>.</returns>
        public static int ToInt32(this object obj)
            => int.TryParse(obj?.ToString(), out var value) ? value : 0;

        /// <summary>
        /// 将对象转换为Double.
        /// </summary>
        /// <param name="obj">对象.</param>
        /// <returns><see cref="double"/>.</returns>
        public static double ToDouble(this object obj)
            => double.TryParse(obj?.ToString(), out var value) ? value : 0d;

        /// <summary>
        /// 将Hash文本对象转换为颜色.
        /// </summary>
        /// <param name="obj">颜色哈希.</param>
        /// <returns><see cref="Windows.UI.Color"/>.</returns>
        public static Color ToColor(this string obj)
        {
            obj = obj.Replace("#", string.Empty);
            obj = Convert.ToInt32(obj).ToString("X2");

            var color = default(Color);
            if (obj.Length == 4)
            {
                obj = "00" + obj;
            }

            if (obj.Length == 6)
            {
                color.R = byte.Parse(obj.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                color.G = byte.Parse(obj.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                color.B = byte.Parse(obj.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                color.A = 255;
            }

            if (obj.Length == 8)
            {
                color.R = byte.Parse(obj.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                color.G = byte.Parse(obj.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                color.B = byte.Parse(obj.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                color.A = byte.Parse(obj.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return color;
        }

        /// <summary>
        /// 将值转换为水平方向的边距.
        /// </summary>
        /// <param name="value">值.</param>
        /// <returns>边距.</returns>
        public static Thickness ToHorizontalPadding(this double value)
            => new Thickness(value, 0, value, 0);

        /// <summary>
        /// 将值转换为顶部边距.
        /// </summary>
        /// <param name="value">值.</param>
        /// <returns>边距.</returns>
        public static Thickness ToTopPadding(this double value)
            => new Thickness(0, value, 0, 0);
    }
}
