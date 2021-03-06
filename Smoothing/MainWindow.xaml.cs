using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Smoothing.ViewModels;
using Smoothing.Helpers;

namespace Smoothing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Test.TestAccesebilty();

            var viewModel = new BlurViewModel(new JpgDialogImageLoader(), 
                new GaussianBlur(), new CustomMessageBox(this));
            var view = new BlurUserControl(viewModel);
            Content = view;
            
            
        }
    }
}
