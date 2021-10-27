using Microsoft.Win32;//
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

using Smoothing.Helpers;

using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Reflection;
using System.Drawing.Imaging;
using System.Windows;

namespace Smoothing.ViewModels
{

    public class BlurViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }



        private WriteableBitmap _currentImage; //Изображение, выводящееся на View, подлежащее обработке
        private WriteableBitmap _loadedImage; //Базовое загруженное изображение, для "отката" без повторной файла

        
        ///private delegate byte[] ImageDataConverter(WriteableBitmap bitmap);


        private bool _IsImageAvailable = false; 
        private int _blur_level;            //Уровень сглаживания

        public RelayCommand AddCommand { get; }
        public RelayCommand BlurCommand { get; }

        public BlurViewModel()
        {
            ///            private ImageDataConverter imageDataConverter = WBImage.ConvertFromWBToBytesArray;
            AddCommand = new RelayCommand(LoadImage, ()=> { return true; } ); //() => { return true; }
            BlurCommand = new RelayCommand(GaussBlurImage, () => { return IsImageAvailable; } ); // По хорошему return IsImageAvailable, но не работает
        } 

        /*Реализации свойств*/
        public bool IsImageAvailable
        //Возможно, лучше поставить флаг, например в случае, когда нужно заблокировать обработку
        // изображения, но ImageData не занулена
        {
            get { return _IsImageAvailable; }
            set
            {
                if (_IsImageAvailable == value)
                {
                    return;
                }

                _IsImageAvailable = value;
                OnPropertyChanged(nameof(_IsImageAvailable));
            }
        }

        public WriteableBitmap LoadedImage //Последнее загруженное изображение
        {
            get { return _loadedImage; }
            set
            {
                if (_loadedImage != value)
                {
                    _loadedImage = value;
                    //CurrentImage.Clone(LoadedImage);

                    OnPropertyChanged(nameof(_loadedImage));
                }
            }
        }

        public WriteableBitmap CurrentImage //Рабочее изображение
        {
            get { return _currentImage; }
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(_currentImage));
                    //BlurCommand.InvalidateCanExecute();
                }
            }
        }



        public int BlurLevel
        {
            get { return _blur_level; }
            set
            {
                if (_blur_level != value)
                {
                    if (value < 0 || value > 50)
                        throw new ArgumentOutOfRangeException(
                              $"Уровень сглаживания должен быть в пределах от 0 до 50");

                    _blur_level = value;
                    OnPropertyChanged(nameof(_blur_level));
                }
            }
        }

        /*Обработчики команд*/
        void LoadImage()
        {
            try
            {
                var path = SelectFile();
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = LoadBytesFromFile(path);
                    LoadWBImage(bytes); 
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        void GaussBlurImage()
        {
            try
            {
                //GaussianBlur blur = new GaussianBlur(WBImage.ConvertFromBytesArrayToWB(LoadedImage.ImageData));
                //CurrentImage.ImageData = LoadedImage.ImageData.Clone();
                
                GaussianBlur blur = new GaussianBlur(LoadedImage.Clone());
                CurrentImage = blur.BlurImage(BlurLevel);
                OnPropertyChanged(nameof(CurrentImage));
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        /*Прочие вспомогательные функции*/
        string SelectFile()
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
        byte[] LoadBytesFromFile(string path)
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

        void LoadWBImage(byte[] bytes)
        {
            try
            {
                LoadedImage = new WriteableBitmap(WBImage.ConvertFromBytesArrayToWB(bytes));

                if (CurrentImage == null)
                {
                    CurrentImage = new WriteableBitmap(LoadedImage);
                }

                CurrentImage = LoadedImage.Clone(); //Можно ли сделать адекватнее?
            }
            catch
            { }
            finally
            {
                OnPropertyChanged(nameof(LoadedImage));
                OnPropertyChanged(nameof(CurrentImage));

                IsImageAvailable = true;
                OnPropertyChanged(nameof(IsImageAvailable));
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if ((e.Text) == null || !(e.Text).All(char.IsDigit))
            {
                e.Handled = true;
            }
        }
    }

    

}
