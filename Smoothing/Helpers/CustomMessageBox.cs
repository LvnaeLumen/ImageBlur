using Smoothing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Smoothing.Helpers
{
    class CustomMessageBox : IMessageBox
    {
        private readonly Window _parent;

        public CustomMessageBox(Window parent)
        {
            _parent = parent;
        }

        public void Show(string message)
        {
            System.Windows.MessageBox.Show(_parent, message);
        }
    }
}
