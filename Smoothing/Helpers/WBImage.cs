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
 /*       public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        private byte[] imageData;
        private WriteableBitmap wbImage;

        
        public void Clone(WBImage source)
        {
            this.imageData = new byte[source.imageData.Length];
            source.imageData.CopyTo(this.imageData, 0);

            this.wbImage = new WriteableBitmap(source.WbImage.Clone());                                 
        }

        public WBImage(WBImage source)
        {
            Clone(source);
        }

        public WBImage(byte[] bytes, WriteableBitmap wb)
        {
            this.imageData = new byte[bytes.Length];
            bytes.CopyTo(this.imageData, 0);

            this.wbImage = new WriteableBitmap(wb.Clone());
        }

        

        public byte[] ImageData
        {
            get { return imageData; }
            set
            {
                if (imageData != value)
                {
                    imageData = value;
                    OnPropertyChanged(nameof(imageData));
                }
            }
        }

        public WriteableBitmap WbImage
        {
            get { return wbImage; }
            set
            {
                if (wbImage != value)
                {
                    wbImage = value;
                    OnPropertyChanged(nameof(imageData));
                }
            }
            //get { return ConvertFromBytesArrayToWB(this.imageData); }
        }
*/
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
            var width = wbitmapSource.PixelWidth;
            var height = wbitmapSource.PixelHeight;
            var stride = width * ((wbitmapSource.Format.BitsPerPixel + 7) / 8);

            var bitmapData = new byte[height * stride];

            wbitmapSource.CopyPixels(bitmapData, stride, 0);
            return bitmapData;
        }

    }
}
