using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


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

        internal object FromFile(object fileName)
        {
            throw new NotImplementedException();
        }


        /*        private string fileName;

                public string FileName
                {
                    get { return fileName; }
                    set
                    {
                        if (fileName != value)
                        {
                            fileName = value;
                            OnPropertyChanged(nameof(FileName));
                        }
                   */
    }
}

  /*      private Bitmap image;
        public Bitmap Image
        {
            get { return image; }
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }
*/
