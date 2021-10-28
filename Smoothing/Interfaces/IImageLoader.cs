using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoothing.Interfaces
{
    public interface IImageLoader
    {
       public bool CanLoad();
       public byte[] LoadImage();
    }
}
