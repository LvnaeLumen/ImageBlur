using Smoothing.Helpers;
using Smoothing.Interfaces;
using Smoothing.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Smoothing
{
    class Test
    {
        //Классы для тестирования
        class TestImageLoader : IImageLoader
        {
            public byte[] LoadImage()
            {
                using (FileStream fs = new FileStream("test1_original.jpg", FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);
                    return bytes;
                }
            }

            public bool CanLoad()
            { return true; }
        }

        class TestMessageBox : IMessageBox
        {
            public void Show(string message)
            {
                //
            }
        }


        public static void TestAccesebilty()
        {
            var imageLoader = new TestImageLoader();
            
            var viewModel = new BlurViewModel(imageLoader,  
                new GaussianBlur(), new TestMessageBox());


            if(viewModel.BlurCommand.CanExecute(null) != false)
            {
                throw new Exception("Blur image command CanExecute is not false");
            }
            
            viewModel.IsImageAvailable = true;
            
            if (viewModel.BlurCommand.CanExecute(null) == false)
            {
                throw new Exception("Blur image command CanExecute is not false");
            }

            viewModel.LoadedImage = new byte[] { 1, 2, 3, 4 };
            viewModel.CurrentImage = new byte[] { 1, 2, 3, 4 };

            viewModel.BlurLevel = -10;

            if (viewModel.BlurLevel < 0)
            {
                throw new Exception("Blur level can't be a negative number");
            }


            viewModel.AddCommand.Execute(null);
            //Тестовая реализация с загрузкой готового файла
            viewModel.BlurLevel = 10;
            viewModel.BlurCommand.Execute(null);
            

            using (FileStream fs = new FileStream("test1_blurred_w10.jpg", FileMode.Open, FileAccess.Read))
            {
                byte[] testBlurredImage = viewModel.CurrentImage;

                byte[] blurredImage = new byte[fs.Length];
                fs.Read(blurredImage, 0, blurredImage.Length);
                

                if (testBlurredImage == null)
                    throw new Exception("Produced blurred image is missing");
                if (testBlurredImage.Length != blurredImage.Length) 
                    throw new Exception("Test blurred image and produced image are not equal");

                
                
                for (int i = 0; i < blurredImage.Length; i++) //0 IQ решение
                {
                    if (testBlurredImage[i] != blurredImage[i])
                        throw new Exception("Test blurred image and produced image are not equal");
                }                
                
                //if (errorCount > 100) //3 IQ Решение
                //


            }

        }
    }
}
