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
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
            SystemNavigationManager m = SystemNavigationManager.GetForCurrentView();
            m.BackRequested += Quit;
            //设置背景
            ApplicationDataContainer localSettings =ApplicationData.Current.LocalSettings;
            try {

                if ((bool)localSettings.Values["IsBackgroundSetting"])
                {
                    ImageBrush imgbrush = new ImageBrush();
                    imgbrush.ImageSource = new BitmapImage(new Uri(@"ms-appx:///Assets/CalBackground.jpg"));
                    Background = imgbrush;
                    localSettings.Values["IsBackgroundSetting"] = true;
                }
                else
                {
                    SolidColorBrush sBrush = new SolidColorBrush();
                    sBrush.Color = Colors.White;
                    Background = sBrush;
                }
            }
            catch
            {
                SolidColorBrush sBrush = new SolidColorBrush();
                sBrush.Color = Colors.White;
                Background = sBrush;
                localSettings.Values["IsBackgroundSetting"] = false;
            }
            //this.Background = null;
            //Window.Current.SizeChanged += Current_SizeChanged;
            //ApplicationView.PreferredLaunchViewSize = new Size { Height = 800, Width = 450 };
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            //ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 450, Height = 800 });

        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            double width = e.Size.Width;
            double height = e.Size.Height;
            if (width > (height * 9 / 16))
            {
                Height = height;
                Width = height * 9.0 / 16.0;
            }
            else
            {
                Height = width * 16.0 / 9.0;
                Width = width;
            }
            //ApplicationView.GetForCurrentView().TryResizeView(new Size { Width = width, Height = height });            


        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine(ApplicationData.Current.RoamingStorageQuota);      
        }

        private async void Quit(object sender, BackRequestedEventArgs e)
        {
            try
            {
                if (VM.RecordList.Count > 0)
                {
                    await VM.BackupAsync(SaveType.RoamingData);
                }
            }
            catch { }
            App.Current.Exit();
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spltvMain.IsPaneOpen = false;
            VM.IsBackVisible = false;
            if (lstiMain.IsSelected)
            {
                VM.UserInputPwd = "";
                VM.IsCheck = false;
                VM.IsUcItemDetailVisible = false;
                VM.Title = "主页";
                grdPwdsList.Visibility = Visibility.Collapsed;
                //grdSet.Visibility = Visibility.Collapsed;
            }
            if (lstiList.IsSelected)
            {
                VM.Title = "收藏列表";
                VM.IsUcItemDetailVisible = false;
                grdPwdsList.Visibility = Visibility.Visible;
                //grdSet.Visibility = Visibility.Collapsed;
            }
            if (lstiAdd.IsSelected)
            {
                VM.Title = "添加条目";
                VM.IsUcItemDetailVisible = false;
                grdPwdsList.Visibility = Visibility.Collapsed;
                //grdSet.Visibility = Visibility.Collapsed;
                VM.GoToAdd();

            }
            if (lstiSet.IsSelected)
            {
                VM.Title = "设置";
                VM.IsUcItemDetailVisible = false;
                grdPwdsList.Visibility = Visibility.Collapsed;
                //grdSet.Visibility = Visibility.Visible;
                VM.SettingResult = "";
            }
            if(lstiRate.IsSelected)
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9nblggh4vgv2"));
            }
            if (lstiQuit.IsSelected)
            {
                try
                {
                    if (VM.RecordList.Count > 0)
                    {
                        await VM.BackupAsync(SaveType.RoamingData);
                    }
                }
                catch { }
                App.Current.Exit();
            }
        }

        private void btnHamburg_Click(object sender, RoutedEventArgs e)
        {
            spltvMain.IsPaneOpen = !spltvMain.IsPaneOpen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn=(Button)sender;
            RecordItem record = (RecordItem)btn.DataContext;
            string contentStr = btn.Content.ToString();
            switch(contentStr)
            {
                case "修改":
                    VM.GoToModify(record);
                    break;
                case "删除":
                    VM.DeleteData(record);
                    break;
                default:break;
                    
            }
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Debug.WriteLine(sender.ToString());
            StackPanel stcp = (StackPanel)sender;
            StackPanel stcpItemHide = new StackPanel();
            foreach (var item in stcp.Children)
            {
                if (item.ToString() == "Windows.UI.Xaml.Controls.StackPanel")
                { stcpItemHide = (StackPanel)item; }
            }
            stcpItemHide.Visibility = stcpItemHide.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        //更换背景
        public void ChangeBackGround()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if ((bool)localSettings.Values["IsBackgroundSetting"])
            {
                SolidColorBrush sBrush = new SolidColorBrush();
                sBrush.Color = Colors.White;
                Background = sBrush;
            }
            else
            {
                ImageBrush imgbrush = new ImageBrush();
                imgbrush.ImageSource = new BitmapImage(new Uri(@"ms-appx:///Assets/CalBackground.jpg"));
                Background = imgbrush;
                localSettings.Values["IsBackgroundSetting"] = true;

            }
        }
    }
}
#region 双击返回代码
//public MainPage()
//{
//    this.InitializeComponent();
//    var m = SystemNavigationManager.GetForCurrentView();
//    m.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
//    m.BackRequested += M_BackRequested;
//}

//private void M_BackRequested(object sender, BackRequestedEventArgs e)
//{
//    Frame rootFrame = Window.Current.Content as Frame;
//    if (rootFrame == null)
//        return;

//    // Navigate back if possible, and if the event has not 
//    // already been handled .
//    if (rootFrame.CanGoBack && e.Handled == false)
//    {
//        e.Handled = true;
//        rootFrame.GoBack();
//    }
//    else if (!rootFrame.CanGoBack && e.Handled == false)
//    {
//        if (b)
//        {
//            App.Current.Exit();
//        }
//        else
//        {
//            b = true;
//            Task.Run(async () =>
//            {
//                await Task.Delay(1500);
//                b = false;
//            });
//        }

//    }
//}
//bool b = false;
#endregion
