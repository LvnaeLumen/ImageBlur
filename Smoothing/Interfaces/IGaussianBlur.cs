using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Smoothing.Interfaces
{
    public interface IGaussianBlur
    {
        public byte[] BlurImage(byte[] sourceImage, int depth);
    }
}
