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
            {
                stkpDetail.Visibility = Visibility.Collapsed;
                btnDetail.Content = "展开";
            }
            else
            {
                stkpDetail.Visibility = Visibility.Visible;
                btnDetail.Content = "收起";
            }
            //Debug.WriteLine(grdDataItem.ActualWidth);
        }

        //修改项目，读到当前所在的父控件，不断向上，找到VM，调用其ModifyIn方法。
        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(this.DataContext);
            var a = VisualTreeHelper.GetParent(this);
            a = VisualTreeHelper.GetParent(a);
            a = VisualTreeHelper.GetParent(a);
            //a = VisualTreeHelper.GetParent(a);
            //a = VisualTreeHelper.GetParent(a);
            //Debug.WriteLine(a);
            //Debug.WriteLine(((ItemsStackPanel)a).DataContext);
            ((ViewModel)((ItemsStackPanel)a).DataContext).ModifyIn(this.DataContext);
        }

    }
}
