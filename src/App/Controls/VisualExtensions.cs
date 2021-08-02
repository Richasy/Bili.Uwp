// Copyright (c) Richasy. All rights reserved.

using Windows.UI;
using Windows.UI.Composition;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视觉扩展.
    /// </summary>
    public static class VisualExtensions
    {
        /// <summary>
        /// 绑定大小.
        /// </summary>
        /// <param name="target">目标元素.</param>
        /// <param name="source">来源视图.</param>
        public static void BindSize(this Visual target, Visual source)
        {
            var exp = target.Compositor.CreateExpressionAnimation("host.Size");
            exp.SetReferenceParameter("host", source);
            target.StartAnimation("Size", exp);
        }

        /// <summary>
        /// 绑定中心点.
        /// </summary>
        /// <param name="target">目标视图.</param>
        public static void BindCenterPoint(this Visual target)
        {
            var exp = target.Compositor.CreateExpressionAnimation("Vector3(this.Target.Size.X / 2, this.Target.Size.Y / 2, 0f)");
            target.StartAnimation("CenterPoint", exp);
        }

        /// <summary>
        /// 将字符串转换为颜色.
        /// </summary>
        /// <param name="str">颜色.</param>
        /// <returns><see cref="Windows.UI.Color"/>.</returns>
        public static Color ToColor(this string str)
        {
            str = str.Replace("#", string.Empty);
            if (int.TryParse(str, out var c))
            {
                str = c.ToString("X2");
            }

            var color = default(Color);
            if (str.Length <= 6)
            {
                str = str.PadLeft(6, '0');
                color.R = byte.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                color.G = byte.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                color.B = byte.Parse(str.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                color.A = 255;
            }
            else
            {
                str = str.PadLeft(8, '0');
                color.R = byte.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                color.G = byte.Parse(str.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                color.B = byte.Parse(str.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                color.A = byte.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            }

            return color;
        }
    }
}
