using System;
using System.Drawing.Imaging;
using System.Drawing;

namespace Moonlit.Drawing
{
    public static class Thumbnail
    {
        #region Methods

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="thumbnailImageWidth">缩略图的宽度（高度与按源图片比例自动生成）</param>
        /// <returns></returns>
        public static System.Drawing.Image CreateThumbnailImage(System.Drawing.Image image, int thumbnailImageWidth)
        {
            int num = ((thumbnailImageWidth / 4) * 3);
            int width = image.Width;
            int height = image.Height;
            //计算图片的比例
            if ((((double)width) / ((double)height)) >= 1.3333333333333333f)
            {
                num = ((height * thumbnailImageWidth) / width);
            }
            else
            {
                thumbnailImageWidth = ((width * num) / height);
            }
            if ((thumbnailImageWidth < 1) || (num < 1))
            {
                throw new Exception("thumbnailImageWidth 太小或 num 太小");
            }
            // 用指定的大小和格式初始化 Bitmap 类的新实例
            Bitmap bitmap = new Bitmap(thumbnailImageWidth, num, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.Transparent);
            graphics.DrawImage(image, new Rectangle(0, 0, thumbnailImageWidth, num));
            return bitmap;
        }
        #endregion
    }
}
