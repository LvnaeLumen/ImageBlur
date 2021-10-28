using Smoothing.ViewModels;
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

namespace Smoothing
{
    /// <summary>
    /// Interaction logic for BlurUserControl.xaml
    /// </summary>
    public partial class BlurUserControl : UserControl
    {
        public BlurUserControl(BlurViewModel viewModel)
        {
            InitializeComponent();

            Loaded += delegate { blur_LayoutRoot.DataContext = viewModel; };
            Unloaded += delegate { blur_LayoutRoot.DataContext = null; }
;        }
    }
}
