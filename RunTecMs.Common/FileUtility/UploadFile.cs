using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace RunTecMs.Common
{
    public class UploadFile
    {

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string UploadAttachment(HttpRequest request)
        {
            HttpPostedFile file = request.Files["fileData"];

            string uploadPath = GetUploadPath("UploadAttachmentPath");

            if (string.IsNullOrEmpty(uploadPath) || file == null)
            {
                return "";
            }

            string uploadFolder = string.Format(uploadPath + "{0}/", DateTime.Now.ToString("yyyy-MM-dd"));
            string path = HttpContext.Current.Server.MapPath(uploadFolder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string ext = Path.GetExtension(file.FileName).ToLower();
            string fileName = path + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;

            try
            {
                file.SaveAs(fileName);
                return fileName;
            }
            catch (Exception)
            {
                // ignored
            }
            return "";
        }

        /// <summary>
        /// 计算缩略图大小
        /// </summary>
        /// <param name="width">原始宽度</param>
        /// <param name="height">原始高度</param>
        /// <param name="maxWidth">最大新宽度</param>
        /// <param name="maxHeight">最大新高度</param>
        /// <param name="isConvert"></param>
        /// <returns></returns>
        private static Size GetThumbnailSize(int width, int height, int maxWidth, int maxHeight, ref bool isConvert)
        {
            decimal MAX_WIDTH = (decimal)maxWidth;
            decimal MAX_HEIGHT = (decimal)maxHeight;
            decimal ASPECT_RATIO = MAX_WIDTH / MAX_HEIGHT;

            int newWidth, newHeight;
            decimal originalWidth = (decimal)width;
            decimal originalHeight = (decimal)height;

            if (originalWidth > MAX_WIDTH || originalHeight > MAX_HEIGHT)
            {
                decimal factor;
                // determine the largest factor 
                if (originalWidth / originalHeight > ASPECT_RATIO)
                {
                    factor = originalWidth / MAX_WIDTH;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
                else
                {
                    factor = originalHeight / MAX_HEIGHT;
                    newWidth = Convert.ToInt32(originalWidth / factor);
                    newHeight = Convert.ToInt32(originalHeight / factor);
                }
            }
            else
            {
                newWidth = width;
                newHeight = height;
                isConvert = false;
            }
            return new Size(newWidth, newHeight);
        }

        /// <summary>
        /// 计算中图大小
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="maxWidth"></param>
        /// <param name="isConvert"></param>
        /// <returns></returns>
        private static Size GetMiddleSize(int width, int height, int maxWidth, ref bool isConvert)
        {
            decimal MAX_WIDTH = (decimal)maxWidth;

            int newWidth, newHeight;
            decimal originalWidth = (decimal)width;
            decimal originalHeight = (decimal)height;

            if (maxWidth < originalWidth)
            {
                newWidth = maxWidth;
                newHeight = Convert.ToInt32(originalHeight / (originalWidth / MAX_WIDTH));
            }
            else
            {
                newWidth = width;
                newHeight = height;
                isConvert = false;
            }
            return new Size(newWidth, newHeight);
        }

        /// <summary>
        /// 给图片添加标记
        /// </summary>
        /// <param name="img"></param>
        /// <param name="filename"></param>
        /// <param name="watermarkText"></param>
        /// <param name="watermarkPosition"></param>
        private static void AddImageSignText(Image img, string filename, string watermarkText, int watermarkPosition)
        {
            Graphics g = Graphics.FromImage(img);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Font drawFont = new Font("Tahoma", ((float)img.Width * (float).03), FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkPosition)
            {
                case 3:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = ((float)img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 2:
                    xpos = ((float)img.Width * (float).50) - (crSize.Width / 2);
                    ypos = (float)img.Height - crSize.Height;
                    break;
                case 1:
                    xpos = ((float)img.Width * (float).99) - crSize.Width;
                    ypos = (float)img.Height - crSize.Height;
                    break;
            }

            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.White), xpos + 1, ypos + 1);
            g.DrawString(watermarkText, drawFont, new SolidBrush(Color.Black), xpos, ypos);


            img.Save(filename);

            g.Dispose();
            img.Dispose();
        }

        /// <summary>
        /// 生成指定大小的图片
        /// </summary>
        /// <param name="original"></param>
        /// <param name="newFileName"></param>
        /// <param name="newSize"></param>
        private static void MakeReSizeImage(Image original, string newFileName, Size newSize)
        {
            //新建一个bmp图片 
            Image bitmap = new Bitmap(newSize.Width, newSize.Height);

            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(original, 0, 0, newSize.Width, newSize.Height);

            // 如果目录不存在则创建
            string dir = Path.GetDirectoryName(newFileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            //// 测试权限
            //var permissionSet = new PermissionSet(PermissionState.None);
            //var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, newFileName);
            //permissionSet.AddPermission(writePermission);

            //if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
            //{
            //    using (FileStream fstream = new FileStream(newFileName, FileMode.Create))
            //    using (TextWriter writer = new StreamWriter(fstream))
            //    {
            //        writer.WriteLine("sometext");
            //    }
            //}
            //else
            //{
            //    //没有权限执行的操作
            //    int i = 0;
            //}

            try
            {
                if (GetFormat(newFileName) == ImageFormat.Gif)
                {
                    bitmap.Save(newFileName, ImageFormat.Jpeg);
                }
                else
                {
                    bitmap.Save(newFileName, GetFormat(newFileName));
                }
            }
            catch (Exception e)
            {
                bitmap.Dispose();
                g.Dispose();
                throw new Exception();
            }
            finally
            {
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 得到图片格式
        /// </summary>
        /// <param name="name">文件名称</param>
        /// <returns></returns>
        private static ImageFormat GetFormat(string name)
        {
            string ext = name.Substring(name.LastIndexOf(".") + 1);
            switch (ext.ToLower())
            {
                case "jpg":
                case "jpeg":
                    return ImageFormat.Jpeg;
                case "bmp":
                    return ImageFormat.Bmp;
                case "png":
                    return ImageFormat.Png;
                case "gif":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// byte[]转换成Image
        /// </summary>
        /// <param name="byteArrayIn">二进制图片流</param>
        /// <returns>Image</returns>
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null) return null;
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                ms.Flush();
                return returnImage;
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            string strpath = GetMapPath(filePath);
            if (File.Exists(strpath))
            {
                //如果存在则删除
                File.Delete(strpath);
            }
        }

        /// <summary>
        /// 获取上传路径
        /// </summary>
        /// <param name="uploadPath">上传路径(web.config中的key)</param>
        /// <returns></returns>
        public static string GetUploadPath(string uploadPath)
        {
            return ConfigHelper.GetConfigString(uploadPath);
        }

    }
}
