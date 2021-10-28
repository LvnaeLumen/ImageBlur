using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Smoothing.Helpers
{
    
    public class WBImage
    {
        public static WriteableBitmap ConvertFromBytesArrayToWB(byte[] imageData)
        {

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = new MemoryStream(imageData);
            bmp.EndInit();
            var buff = new WriteableBitmap(bmp);

            return buff;

        }

        public static byte[] ConvertFromWBToBytesArray(WriteableBitmap wbitmapSource)
        {
            //var bytes = value as byte[];
            
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create((BitmapSource)wbitmapSource));
                enc.Save(outStream);
                var ret = outStream.ToArray();
                return ret;
            }
        }

    }
}
