using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LocalTool
{
    /// <summary>
    /// gif图片展示处理
    /// </summary>
    public class GifImage : System.Windows.Controls.Image
    {
        /// <summary>
        /// gif动画的System.Drawing.Bitmap
        /// </summary>
        private Bitmap gifBitmap;

        /// <summary>
        /// 用于显示每一帧的BitmapSource
        /// </summary>
        private BitmapSource bitmapSource;

        public GifImage(string path)
        {
            this.gifBitmap = new Bitmap(path);
            this.bitmapSource = this.GetBitmapSource();
            this.Source = this.bitmapSource;
        }

        /// <summary>
        /// 从System.Drawing.Bitmap中获得用于显示的那一帧图像的BitmapSource
        /// </summary>
        /// <returns></returns>
        private BitmapSource GetBitmapSource()
        {
            IntPtr handle = IntPtr.Zero;

            try
            {
                handle = this.gifBitmap.GetHbitmap();
                this.bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    DeleteObject(handle);
                }
            }
            return this.bitmapSource;
        }

        /// <summary>
        /// 开始动画 
        /// </summary>
        public void StartAnimate()
        {
            ImageAnimator.Animate(this.gifBitmap, this.OnFrameChanged);
        }

        /// <summary>
        /// 停止动画
        /// </summary>
        public void StopAnimate()
        {
            ImageAnimator.StopAnimate(this.gifBitmap, this.OnFrameChanged);
        }

        /// <summary>
        /// Event handler for the frame changed
        /// </summary>
        private void OnFrameChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                ImageAnimator.UpdateFrames(); // 更新到下一帧
                if (this.bitmapSource != null)
                {
                    this.bitmapSource.Freeze();
                }

                //Convert the bitmap to BitmapSource that can be display in WPF Visual Tree
                this.bitmapSource = this.GetBitmapSource();
                Source = this.bitmapSource;
                this.InvalidateVisual();
            }));
        }

        /// <summary>
        /// Delete local bitmap resource
        /// Reference: http://msdn.microsoft.com/en-us/library/dd183539(VS.85).aspx
        /// </summary>
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject(IntPtr hObject);
    }
    public static class CImageTool
    {

        public static DrawingImage ReadSvgImage(string path, string name)
        {
            string svgPath = path + name + ".svg";
            DrawingImage image = new DrawingImage();
            SharpVectors.Renderers.Wpf.WpfDrawingSettings settings = new SharpVectors.Renderers.Wpf.WpfDrawingSettings();
            settings.IncludeRuntime = true;
            SharpVectors.Converters.FileSvgReader reader = new SharpVectors.Converters.FileSvgReader(settings);
            DrawingGroup group;
            FileStream stream = null;
            try
            {
                stream = new FileStream(svgPath, FileMode.Open, FileAccess.Read);
                group = reader.Read(stream);
                image.Drawing = group;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return image;
        }
        public static DrawingImage ReadSvgImage(string pathName)
        {
            string svgPath = pathName;
            DrawingImage image = new DrawingImage();
            SharpVectors.Renderers.Wpf.WpfDrawingSettings settings = new SharpVectors.Renderers.Wpf.WpfDrawingSettings();
            settings.IncludeRuntime = true;
            SharpVectors.Converters.FileSvgReader reader = new SharpVectors.Converters.FileSvgReader(settings);
            DrawingGroup group;
            FileStream stream = null;
            try
            {
                stream = new FileStream(svgPath, FileMode.Open, FileAccess.Read);
                group = reader.Read(stream);
                image.Drawing = group;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return image;
        }
        /// <summary>
        /// 2020.01.19 武琦玮 
        /// 目的： 通过相对路径读取外部配置文件夹中的图片
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BitmapImage GetIconImage(string path)
        {
            BitmapImage Icon = new BitmapImage();
            string relative_path = string.Empty;
            if (File.Exists(path))
            {
                relative_path = "pack://siteoforigin:,,,/" + path;
            }
            return Icon = new BitmapImage(new Uri(relative_path, UriKind.RelativeOrAbsolute));
        }
    }
}
