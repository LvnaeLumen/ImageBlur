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
using Smoothing.Interfaces;

namespace Smoothing.ViewModels
{

    public class BlurViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }



        private byte[] _currentImage; //Изображение, выводящееся на View, подлежащее обработке
        private byte[] _loadedImage; //Базовое загруженное изображение, для "отката" без повторной файла
        private readonly IImageLoader _imageLoader;
        private readonly IGaussianBlur _gaussianBlur;
        private readonly IMessageBox _messageBox;

        private bool _isImageAvailable = false; 
        private int _blur_level;            //Уровень сглаживания

        public ICommand AddCommand { get; }
        public ICommand BlurCommand { get; }

        public BlurViewModel(IImageLoader imageLoader, IGaussianBlur gaussianBlur, IMessageBox messageBox)
        {
            ///            private ImageDataConverter imageDataConverter = WBImage.ConvertFromWBToBytesArray;
            _imageLoader = imageLoader;
            _gaussianBlur = gaussianBlur;
            _messageBox = messageBox;

            AddCommand = new RelayCommand(LoadImage, ()=> { return imageLoader.CanLoad(); } );
            //Возможно не лучшее решение, но логика следующая: Если, например, будет использован класс,
            //выполняющй загрузку изображения после подключения к веб-серверу, и ожидающий ответа и реализующий CanLoad
            BlurCommand = new RelayCommand(GaussBlurImage, () => { return _isImageAvailable; } );
 
        }



        /*Реализации свойств*/
        /*private bool IsImageAvailable
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
        }*/

        public byte[] LoadedImage //Последнее загруженное изображение
        {
            get { return _loadedImage; }
            set
            {
                if (_loadedImage != value)
                {
                    _loadedImage = value;                    

                    OnPropertyChanged(nameof(_loadedImage));

                }
            }
        }

        public byte[] CurrentImage //Рабочее изображение
        {
            get { return _currentImage; }
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(_currentImage));                    
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
                    if (value < 0)
                    {

                        _messageBox.Show("Blur depth must be a positive number");
                    }
                    else
                    {
                        _blur_level = value;
                        OnPropertyChanged(nameof(_blur_level));
                    }
                }
            }
        }

        

        /*Обработчики команд*/
        void LoadImage()
        {
            try
            {               
                LoadWBImage(_imageLoader.LoadImage());
                /*using (MemoryStream ms = new MemoryStream(LoadedImage))
                {
                    Image ret = Image.FromStream(ms);
                    ret.Save("test1_original.jpg");
                }*/
            }
            catch (Exception e)
            {
                _messageBox.Show(e.ToString());
            }
        }

        void GaussBlurImage()
        {
            try
            {
                CurrentImage = _gaussianBlur.BlurImage(LoadedImage, BlurLevel);
                /*
                using (MemoryStream ms = new MemoryStream(CurrentImage))
                {  
                    Image ret = Image.FromStream(ms);
                    ret.Save("test1_blurred_w10.jpg");
                }*/

                OnPropertyChanged(nameof(CurrentImage));
            }
            catch (Exception e)
            {
                _messageBox.Show(e.ToString());
            }
        }

        /*Прочие вспомогательные функции*/
        

        void LoadWBImage(byte[] bytes)
        {
            try
            {
                LoadedImage = WBImage.ConvertFromWBToBytesArray(WBImage.ConvertFromBytesArrayToWB(bytes));

                CurrentImage = LoadedImage; 
            }
            catch
            { 

            }
            finally
            {
                OnPropertyChanged(nameof(LoadedImage));
                OnPropertyChanged(nameof(CurrentImage));

                _isImageAvailable = true;
                //OnPropertyChanged(nameof(IsImageAvailable));
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
