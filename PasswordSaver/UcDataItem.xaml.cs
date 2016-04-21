using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace PasswordSaver
{
    public sealed partial class UcDataItem : UserControl
    {
        public UcDataItem()
        {
            this.InitializeComponent();
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            if (stkpDetail.Visibility == Visibility.Visible)
                stkpDetail.Visibility = Visibility.Collapsed;
            else
                stkpDetail.Visibility = Visibility.Visible;
            Debug.WriteLine(grdDataItem.ActualWidth);
        }
    }
}
