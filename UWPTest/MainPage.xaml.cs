using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace UWPTest
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent(); 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bImage1 = new BitmapImage(new Uri(@"ms-appx:///Assets/CalBackground.jpg"));
            if (this.Background == null)
            {
                ImageBrush imgbrush = new ImageBrush();
                //imgbrush.ImageSource = new ImageSource(@"Assets/CalBackground.jpg");
                BitmapImage bImage = new BitmapImage(new Uri(@"ms-appx:///Assets/CalBackground.jpg"));
                imgbrush.ImageSource = bImage;
                this.Background = imgbrush;
                return;
            }
            this.Background = null;
        }
    }
}
