using PasswordSaver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace PasswordSaver
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ViewModel VM = new ViewModel();
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = VM;
            FileManager.WriteCode("8888"); 
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            Debug.WriteLine(ApplicationData.Current.RoamingStorageQuota);   
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lstiMain.IsSelected)
            {
                VM.IsCheck = false;
                grdAdd.Visibility = Visibility.Collapsed;
                grdList.Visibility = Visibility.Collapsed;
                grdSet.Visibility = Visibility.Collapsed;
            }
            if (lstiList.IsSelected)
            {
                grdAdd.Visibility = Visibility.Collapsed;
                grdList.Visibility = Visibility.Visible;
                grdSet.Visibility = Visibility.Collapsed;
            }
            if (lstiAdd.IsSelected)
            {
                grdAdd.Visibility = Visibility.Visible;
                grdList.Visibility = Visibility.Collapsed;
                grdSet.Visibility = Visibility.Collapsed;
            }
            if (lstiSet.IsSelected)
            {
                grdAdd.Visibility = Visibility.Collapsed;
                grdList.Visibility = Visibility.Collapsed;
                grdSet.Visibility = Visibility.Visible;
            }
        }

        private void btnHamburg_Click(object sender, RoutedEventArgs e)
        {
            spltvMain.IsPaneOpen = !spltvMain.IsPaneOpen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VM.IsCheck = !VM.IsCheck;
            Debug.WriteLine(VM.UserInputPwd);
        }

        private async void btnList_Click(object sender, RoutedEventArgs e)
        {
            lstvList.Width = grdList.ActualWidth;
            VM.RecordItems.Add(new Models.RecordItem("h", "h", "h", "h"));

            string str = FileManager.GetJsonString<ObservableCollection<RecordItem>>(VM.RecordItems);
            str = DESHelper.DESEncrypt("8888", str);
            await FileManager.WriteToRoamingDataAsync(str);

            string strs =await FileManager.ReadRoamingDataAsync();
            strs = DESHelper.DESDecrypt("8888", strs);
            ObservableCollection<RecordItem> HH = new ObservableCollection<RecordItem>();
            HH = FileManager.ReadFromJson<ObservableCollection<RecordItem>>(strs);

            foreach (RecordItem item in HH)
            {
                Debug.WriteLine(item.WebSite);
                Debug.WriteLine(item.Account);
                Debug.WriteLine(item.Pwd);
                Debug.WriteLine("");
                Debug.WriteLine("");
                Debug.WriteLine("");
            }
        }
    }
}
