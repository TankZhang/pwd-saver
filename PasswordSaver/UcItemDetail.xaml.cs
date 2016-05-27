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
    public sealed partial class UcItemDetail : UserControl
    {
        public UcItemDetail()
        {
            this.InitializeComponent();
        }
        

        private void Hyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            stcpGeneratePwd.Visibility = stcpGeneratePwd.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void btnGenrt_Click(object sender, RoutedEventArgs e)
        {
            string str= "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if(ckbxNum.IsChecked==true)
                str+= "0123456789012345678901234567890123456789";
            if(ckbxPunc.IsChecked==true)
                str+= ";',.?>;',.?><:{}[]~_-=+!@#$%^&()";
            if(ckbxUpLow.IsChecked==true)
                str+= "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz";
            int index;
            if (!int.TryParse(tbxNum.Text, out index))
                index = 8;
            Random r = new Random();
            string pwd = "";
            for (int i = 0; i < index; i++)
            {
                pwd += str[r.Next(str.Length)];
            }

            tbxPwd.Text = pwd;
            stcpGeneratePwd.Visibility = Visibility.Collapsed;
        }
        
        private void tbxNum_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxNum.Text = "";
        }
    }
}
