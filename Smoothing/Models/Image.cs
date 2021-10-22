using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Smoothing.Models
{
    public class MyImage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string filePath;

        private WriteableBitmap wbmap = new  WriteableBitmap(100, 100, 300, 300, PixelFormats.Bgra32, null);


        public string FilePath
        {
            get { return filePath; }
            set
            {
                if (filePath != value)
                {
                    filePath = value;
                    OnPropertyChanged(nameof(FilePath));
                }
            }
        }

        public WriteableBitmap Wbmap
        {
            get { return wbmap; }
            set
            {
                if (wbmap != value)
                {
                    wbmap = value;
                    OnPropertyChanged(nameof(Wbmap));
                }
            }
        }

    }
}
