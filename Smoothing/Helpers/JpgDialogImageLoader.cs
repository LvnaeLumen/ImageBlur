using Microsoft.Win32;
using Smoothing.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smoothing.Helpers
{
    class JpgDialogImageLoader:IImageLoader
    {   
        public bool CanLoad()
        {
            return true;
        }
        public byte[] LoadImage()
        {
            var path = SelectFile();
            
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            byte[] bytes = LoadBytesFromFile(path);
            fs.Close();

            return bytes;
        }
        static string SelectFile()
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();

                openFile.Filter = "jpg files (*.jpg)|*.jpg";
                openFile.FilterIndex = 0;

                if (openFile.ShowDialog() == true)
                {

                    FileInfo info = new FileInfo(openFile.FileName);
                    return openFile.FileName;

                }
                return null;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }
        private static byte[] LoadBytesFromFile(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    return bytes;

                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                return null;
            }
        }

    }
}
