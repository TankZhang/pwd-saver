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
using Windows.UI;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UWPTest
{
    public sealed partial class UcDataItem : UserControl
    {
        public UcDataItem()
        {
            this.InitializeComponent();
        }

        //修改项目，读到当前所在的父控件，不断向上，找到VM，调用其ModifyIn方法。
        private void btnModify_Click(object sender, RoutedEventArgs e)
        {

            ////Debug.WriteLine(this.DataContext);
            var a = VisualTreeHelper.GetParent(this);
            a = VisualTreeHelper.GetParent(a);
            a = VisualTreeHelper.GetParent(a);
            Debug.WriteLine(((ItemsStackPanel)a).DataContext);
            ((TViewModel)((ItemsStackPanel)a).DataContext).GoToModify((RecordItem)this.DataContext);

            ////Debug.WriteLine(a);
            ////Debug.WriteLine(((ItemsStackPanel)a).DataContext);
            //((TViewModel)((ItemsStackPanel)a).DataContext).GoToModify((RecordItem)this.DataContext);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //var a = VisualTreeHelper.GetParent(this);
            //a = VisualTreeHelper.GetParent(a);
            //a = VisualTreeHelper.GetParent(a);
            //((TViewModel)((ItemsStackPanel)a).DataContext).DeleteData((RecordItem)this.DataContext);
        }
       

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (stkpDetail.Visibility == Visibility.Visible)
            {
                stkpDetail.Visibility = Visibility.Collapsed;
                hyplDetail.Content = "详细";
                hyplDetail.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                stkpDetail.Visibility = Visibility.Visible;
                hyplDetail.Content = "收起";
                hyplDetail.Foreground = new SolidColorBrush(Colors.LightBlue);
            }

        }
    }
}
