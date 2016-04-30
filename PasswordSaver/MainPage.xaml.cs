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
            this.InitializeComponent() ;
            this.DataContext = VM;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            Debug.WriteLine(ApplicationData.Current.RoamingStorageQuota);   
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VM.IsBackVisible = false;
            if(lstiMain.IsSelected)
            {
                VM.IsCheck = false;
                VM.IsUcItemDetailVisible = false;
                VM.Title = "主页";
                grdPwdsList.Visibility = Visibility.Collapsed;
                grdSet.Visibility = Visibility.Collapsed;
            }
            if (lstiList.IsSelected)
            {
                VM.Title = "收藏列表";
                VM.IsUcItemDetailVisible = false;
                grdPwdsList.Visibility = Visibility.Visible;
                grdSet.Visibility = Visibility.Collapsed;
            }
            if (lstiAdd.IsSelected)
            {
                VM.Title = "添加条目";
                VM.IsUcItemDetailVisible = false;
                grdPwdsList.Visibility = Visibility.Collapsed;
                grdSet.Visibility = Visibility.Collapsed;

            }
            if (lstiSet.IsSelected)
            {
                VM.Title = "设置";
                VM.IsUcItemDetailVisible = false;
                grdPwdsList.Visibility = Visibility.Collapsed;
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

        private void btnList_Click(object sender, RoutedEventArgs e)
        {
            string timeNow = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            VM.RecordItems.Add(new Models.RecordItem("website-----" + timeNow, "account-----" + timeNow, "pwd-----" + timeNow, "note-----" + timeNow));
            Debug.WriteLine(VM.RecordItems.Count.ToString());
            //VM.RecordItems[3].WebSite = "更改之后的！";
            //string s = "da";
        }

        private async void btnChangeCode_Click(object sender, RoutedEventArgs e)
        {
            FileManager.WriteCode("321");
            string str = FileManager.GetJsonString<ObservableCollection<RecordItem>>(VM.RecordItems);
            str = EncryptHelper.DESEncrypt("321", str);
            await FileManager.WriteToRoamingDataAsync(str);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            VM.IsProgressRingVisible = true;
            string str = FileManager.GetJsonString<ObservableCollection<RecordItem>>(VM.RecordItems);
            str = EncryptHelper.DESEncrypt(VM.RightPwd, str);
            await FileManager.WriteToRoamingDataAsync(str);
            VM.IsProgressRingVisible = false;
        }
        
    }
}
