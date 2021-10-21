using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Drawing;

using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Smoothing.Models;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Reflection;
using System.Drawing.Imaging;

namespace Smoothing.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string bitmap_path = AppDomain.CurrentDomain.BaseDirectory + "temp.jpg";
        private MyImage image;
        //private GaussianBlur GaussianBlur;

        public MyImage LoadedImage
        {
            get { return image; }
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged(nameof(image));
                }
            }
        }

        public ViewModel()
        {
            

        }



        RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(
                    (obj) =>
                    {

                        OpenFileDialog openFile = new OpenFileDialog();

                        openFile.Filter = "jpg files (*.jpg)|*.jpg";
                        openFile.FilterIndex = 0;

                        if (openFile.ShowDialog() == true)
                        {
                            MyImage image = new MyImage();
                            FileInfo info = new FileInfo(openFile.FileName);
                            image.FilePath = openFile.FileName;


                            LoadedImage = image;
                            OnPropertyChanged(nameof(LoadedImage));
                        }
                    }));
            }

        }

        RelayCommand blurCommand;


        public RelayCommand BlurCommand
        {
            get
            {
                return blurCommand ?? (blurCommand = new RelayCommand(
                    (obj) =>
                    {

                        try
                        {
                            if (LoadedImage == null)
                                throw new Exception("Изображение не выбрано");

                            FileStream stream = new FileStream(LoadedImage.FilePath, FileMode.Open, FileAccess.Read);
                            
                            
                            BinaryReader reader = new BinaryReader(stream);
                                
                            
                            var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
                            reader.Close();
                            stream.Close();
                            
                            
                            
                            Bitmap bitmap = new Bitmap(memoryStream);
                            memoryStream.Close();
                            GC.Collect();

                            //Наделал кучу костылей для нормальной записи битмапа в файл,
                            //но все равно иногда, раз в 20 вызовов, выдает ошибку доступа к занятому буфферному файлу


                            GaussianBlur blur = new GaussianBlur(bitmap);
                            bitmap.Dispose();
                            bitmap = null;

                            Bitmap result = blur.Process(10);

                                

                            using (MemoryStream memory = new MemoryStream())
                            {
                                using (FileStream fs = new FileStream(bitmap_path, FileMode.Create, FileAccess.ReadWrite))
                                {
                                    result.Save(memory, ImageFormat.Jpeg);
                                    byte[] bytes = memory.ToArray();
                                    fs.Write(bytes, 0, bytes.Length);
                                }
                            }

                                                                                  

                            

                            /*           result.Save(bitmap_path, ImageFormat.Jpeg);
                                       result.Dispose();*/

                            LoadedImage.FilePath = bitmap_path;
                            OnPropertyChanged(nameof(LoadedImage));

                            

                        }
                        /*
                        {
                            GaussianBlur blur = new GaussianBlur(blur_image as Bitmap);
                            Bitmap result = blur.Process(10);

                            using (MemoryStream memory = new MemoryStream())
                            {
                                using (FileStream fs = new FileStream(bitmap_path, FileMode.Create, FileAccess.ReadWrite))
                                {
                                    result.Save(memory, ImageFormat.Jpeg);
                                    byte[] bytes = memory.ToArray();
                                    fs.Write(bytes, 0, bytes.Length);
                                }
                            }
                        }
                        
                    }*/
                        catch (Exception)
                        { 
                            //В частности отлов ошибок доступа и ошибки при сглаживании уже отображенного сглашенного изображения
                        }
                        
                    }));
            }
        }


    }
}
