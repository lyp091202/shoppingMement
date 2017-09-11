using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using RunTecMs.Common.DBUtility;

namespace RunTecMs.Common.FileUtility
{
    public class UploadFiles : System.Web.UI.Page
    {
        #region 发发API上传图片
        /// <summary>
        /// 发消息时上传图片
        /// </summary>
        /// <param name="isWaterMark">是否加水印</param>
        /// <returns></returns>
        public static string UploadPic(bool isWaterMark)
        {
            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string filename = Path.GetFileName(file.FileName);
                string fileExtname = Path.GetExtension(filename).ToLower();
             
                int ImgUploadSize = Convert.ToInt32(PubConstant.GetConfigString("ImgUploadSize"));
                string ImageUploadPath = PubConstant.GetConfigString("ImgUploadPath");
                if (file.ContentLength > ImgUploadSize)
                {
                    return "";
                }
                if ((fileExtname == ".jpg" || fileExtname == ".jpeg" || fileExtname == ".gif" || fileExtname == ".png") && file.InputStream.Length > 0)
                {
                    Image image = Image.FromStream(file.InputStream);
                    string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                    string backupPath = ImageUploadPath + "Backup/" + dateNow + "/";
                    FileOperate.FolderCreate(GetMapPath(backupPath));
                    string thumPath = ImageUploadPath + "Thum/" + dateNow + "/";
                    FileOperate.FolderCreate(GetMapPath(thumPath));
                    string smallPath = ImageUploadPath + "Small/" + dateNow + "/";
                    FileOperate.FolderCreate(GetMapPath(smallPath));
                    string largePath = ImageUploadPath + "Large/" + dateNow + "/";
                    FileOperate.FolderCreate(GetMapPath(largePath));
                    string middlePath = ImageUploadPath + "Middle/" + dateNow + "/";
                    FileOperate.FolderCreate(GetMapPath(middlePath));
                    string guid = Guid.NewGuid().ToString();
                    //string newFileName = guid + fileExtname;
                    string newFileName = guid + ".jpg";

                    string strpath = GetMapPath(backupPath + newFileName);
                    file.SaveAs(strpath);
                    if (isWaterMark)
                    {
                        string waterMarkText = string.Empty;
                        waterMarkText = "fafa.com";

                        if (fileExtname != ".gif")
                        {
                            AddImageSignText(image, GetMapPath(largePath + newFileName), waterMarkText, 1);
                        }
                        else
                        {
                            image.Save(GetMapPath(largePath + newFileName));
                            image.Dispose();
                        }
                    }
                    else
                    {
                        image.Save(GetMapPath(largePath + newFileName));
                        image.Dispose();
                    }
                    image = Image.FromFile(GetMapPath(largePath + newFileName));
                    int imageWidth = image.Width;
                    int imageHeight = image.Height;
                    bool isConvert = true;
                    Size newSize = GetThumbnailSize(imageWidth, imageHeight, 200, 200, ref isConvert);
                    if (isConvert)
                    {
                        MakeReSizeImage(image, GetMapPath(smallPath + newFileName), newSize);
                    }
                    else
                    {
                        image.Save(GetMapPath(smallPath + newFileName));
                    }

                    newSize = GetThumbnailSize(imageWidth, imageHeight, 120, 120, ref isConvert);
                    if (isConvert)
                    {
                        MakeReSizeImage(image, GetMapPath(thumPath + newFileName), newSize);
                    }
                    else
                    {
                        image.Save(GetMapPath(thumPath + newFileName));
                    }
                    newSize = GetMiddleSize(imageWidth, imageHeight, 440, ref isConvert);
                    if (isConvert)
                    {
                        MakeReSizeImage(image, GetMapPath(middlePath + newFileName), newSize);
                    }
                    else
                    {
                        image.Save(GetMapPath(middlePath + newFileName));
                    }
                    image.Dispose();
                    return middlePath + newFileName;
                }
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        
        /// <summary>
        /// 发消息时上传图片
        /// </summary>
        /// <param name="strImgBase64">图片的Base64编码字符串</param>
        /// <returns></returns>
        public static string UploadPic(string strImgBase64)
        {
            string returnImgUrl = "";

            string strBase64Img = Regex.Replace(strImgBase64, @"data:image/(png|jpeg|gif);base64,", "");
            byte[] byteImg = Convert.FromBase64String(strBase64Img);
            Image image = byteArrayToImage(byteImg);

            string imageUploadPath = PubConstant.GetConfigString("ImgUploadPath");

            try
            {
                int imageWidth = image.Width;
                int imageHeight = image.Height;

                bool isConvert = true;

                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                string backupPath = imageUploadPath + "Backup/" + dateNow + "/";
                FileOperate.FolderCreate(GetMapPath(backupPath));
                string thumPath = imageUploadPath + "Thum/" + dateNow + "/";
                FileOperate.FolderCreate(GetMapPath(thumPath));
                string smallPath = imageUploadPath + "Small/" + dateNow + "/";
                FileOperate.FolderCreate(GetMapPath(smallPath));
                string largePath = imageUploadPath + "Large/" + dateNow + "/";
                FileOperate.FolderCreate(GetMapPath(largePath));
                string middlePath = imageUploadPath + "Middle/" + dateNow + "/";
                FileOperate.FolderCreate(GetMapPath(middlePath));
                string guid = Guid.NewGuid().ToString();
                //string newFileName = guid + fileExt;
                string newFileName = guid + ".jpg";
                returnImgUrl = smallPath + newFileName;

                Size newSize = GetThumbnailSize(imageWidth, imageHeight, imageWidth, imageHeight, ref isConvert);
                MakeReSizeImage(image, GetMapPath(backupPath + newFileName), newSize);

                newSize = GetThumbnailSize(imageWidth, imageHeight, imageWidth, imageHeight, ref isConvert);
                MakeReSizeImage(image, GetMapPath(largePath + newFileName), newSize);

                newSize = GetThumbnailSize(imageWidth, imageHeight, 440, 440, ref isConvert);
                MakeReSizeImage(image, GetMapPath(middlePath + newFileName), newSize);

                newSize = GetThumbnailSize(imageWidth, imageHeight, 200, 200, ref isConvert);
                MakeReSizeImage(image, GetMapPath(smallPath + newFileName), newSize);

                newSize = GetThumbnailSize(imageWidth, imageHeight, 120, 120, ref isConvert);
                MakeReSizeImage(image, GetMapPath(thumPath + newFileName), newSize);

                image.Dispose();



            }
            catch (Exception e)
            {
                image.Dispose();
                return "";
            }

            return returnImgUrl;

        }
        #endregion

        public string UploadFile(string filePath, int maxSize, string[] fileType, System.Web.UI.HtmlControls.HtmlInputFile TargetFile)
        {
            string Result = "UnDefine";
            bool typeFlag = false;
            string FilePath = filePath;
            int MaxSize = maxSize;
            string strFileName, strNewName, strFilePath;
            if (TargetFile.PostedFile.FileName == "")
            {
                return "FILE_ERR";
            }
            strFileName = TargetFile.PostedFile.FileName;
            TargetFile.Accept = "*/*";
            strFilePath = FilePath;
            if (Directory.Exists(strFilePath) == false)
            {
                Directory.CreateDirectory(strFilePath);
            }
            FileInfo myInfo = new FileInfo(strFileName);
            string strOldName = myInfo.Name;
            strNewName = strOldName.Substring(strOldName.LastIndexOf("."));
            strNewName = strNewName.ToLower();
            if (TargetFile.PostedFile.ContentLength <= MaxSize)
            {
                for (int i = 0; i <= fileType.GetUpperBound(0); i++)
                {
                    if (strNewName.ToLower() == fileType[i].ToString()) { typeFlag = true; break; }
                }
                if (typeFlag)
                {
                    string strFileNameTemp = GetUploadFileName();
                    string strFilePathTemp = strFilePath;
                    float strFileSize = TargetFile.PostedFile.ContentLength;
                    strOldName = strFileNameTemp + strNewName;
                    strFilePath = strFilePath + "\\" + strOldName;
                    TargetFile.PostedFile.SaveAs(strFilePath);
                    Result = strOldName + "|" + strFileSize;
                    TargetFile.Dispose();
                }
                else
                {
                    return "TYPE_ERR";
                }
            }
            else
            {
                return "SIZE_ERR";
            }
            return (Result);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filePath">保存文件地址</param>
        /// <param name="maxSize">文件最大大小</param>
        /// <param name="fileType">文件后缀类型</param>
        /// <param name="TargetFile">控件名</param>
        /// <param name="saveFileName">保存后的文件名和地址</param>
        /// <param name="fileSize">文件大小</param>
        /// <returns></returns>
        public string UploadFile(string filePath, int maxSize, string[] fileType, System.Web.UI.HtmlControls.HtmlInputFile TargetFile, out string saveFileName, out int fileSize)
        {
            saveFileName = "";
            fileSize = 0;

            string Result = "";
            bool typeFlag = false;
            string FilePath = filePath;
            int MaxSize = maxSize;
            string strFileName, strNewName, strFilePath;
            if (TargetFile.PostedFile.FileName == "")
            {
                return "请选择上传的文件";
            }
            strFileName = TargetFile.PostedFile.FileName;
            TargetFile.Accept = "*/*";
            strFilePath = FilePath;
            if (Directory.Exists(strFilePath) == false)
            {
                Directory.CreateDirectory(strFilePath);
            }
            FileInfo myInfo = new FileInfo(strFileName);
            string strOldName = myInfo.Name;
            strNewName = strOldName.Substring(strOldName.LastIndexOf("."));
            strNewName = strNewName.ToLower();
            if (TargetFile.PostedFile.ContentLength <= MaxSize)
            {
                string strFileNameTemp = GetUploadFileName();
                string strFilePathTemp = strFilePath;
                strOldName = strFileNameTemp + strNewName;
                strFilePath = strFilePath + "\\" + strOldName;

                fileSize = TargetFile.PostedFile.ContentLength / 1024;
                saveFileName = strFilePath.Substring(strFilePath.IndexOf("FileUpload\\"));
                TargetFile.PostedFile.SaveAs(strFilePath);
                TargetFile.Dispose();
            }
            else
            {
                return "上传文件超出指定的大小";
            }
            return (Result);
        }

        public string UploadFile(string filePath, int maxSize, string[] fileType, string filename, System.Web.UI.HtmlControls.HtmlInputFile TargetFile)
        {
            string Result = "UnDefine";
            bool typeFlag = false;
            string FilePath = filePath;
            int MaxSize = maxSize;
            string strFileName, strNewName, strFilePath;
            if (TargetFile.PostedFile.FileName == "")
            {
                return "FILE_ERR";
            }
            strFileName = TargetFile.PostedFile.FileName;
            TargetFile.Accept = "*/*";
            strFilePath = FilePath;
            if (Directory.Exists(strFilePath) == false)
            {
                Directory.CreateDirectory(strFilePath);
            }
            FileInfo myInfo = new FileInfo(strFileName);
            string strOldName = myInfo.Name;
            strNewName = strOldName.Substring(strOldName.Length - 3, 3);
            strNewName = strNewName.ToLower();
            if (TargetFile.PostedFile.ContentLength <= MaxSize)
            {
                for (int i = 0; i <= fileType.GetUpperBound(0); i++)
                {
                    if (strNewName.ToLower() == fileType[i].ToString()) { typeFlag = true; break; }
                }
                if (typeFlag)
                {
                    string strFileNameTemp = filename;
                    string strFilePathTemp = strFilePath;
                    strOldName = strFileNameTemp + "." + strNewName;
                    strFilePath = strFilePath + "\\" + strOldName;
                    TargetFile.PostedFile.SaveAs(strFilePath);
                    Result = strOldName;
                    TargetFile.Dispose();
                }
                else
                {
                    return "TYPE_ERR";
                }
            }
            else
            {
                return "SIZE_ERR";
            }
            return (Result);
        }

        public string GetUploadFileName()
        {
            string Result = "";
            DateTime time = DateTime.Now;
            Result += time.Year.ToString() + FormatNum(time.Month.ToString(), 2) + FormatNum(time.Day.ToString(), 2) + FormatNum(time.Hour.ToString(), 2) + FormatNum(time.Minute.ToString(), 2) + FormatNum(time.Second.ToString(), 2) + FormatNum(time.Millisecond.ToString(), 3);
            return (Result);
        }

        public string FormatNum(string Num, int Bit)
        {
            int L;
            L = Num.Length;
            for (int i = L; i < Bit; i++)
            {
                Num = "0" + Num;
            }
            return (Num);
        }

        #region 图片处理方法
        /// <summary>
        /// 计算缩略图大小
        /// </summary>
        /// <param name="width">原始宽度</param>
        /// <param name="height">原始高度</param>
        /// <param name="maxWidth">最大新宽度</param>
        /// <param name="maxHeight">最大新高度</param>
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
        /// <param name="maxHeight"></param>
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
        /// <param name="_newSize"></param>
        private static void MakeReSizeImage(Image original, string newFileName, Size _newSize)
        {
            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(_newSize.Width, _newSize.Height);

            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(original, 0, 0, _newSize.Width, _newSize.Height);

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
            catch (System.Exception e)
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
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        /// <summary>
        /// byte[]转换成Image
        /// </summary>
        /// <param name="byteArrayIn">二进制图片流</param>
        /// <returns>Image</returns>
        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null) return null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byteArrayIn))
            {
                System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
                ms.Flush();
                return returnImage;
            }
        }
        #endregion
    }
}
