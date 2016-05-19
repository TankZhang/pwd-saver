using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace RoamingTest
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            grdMain.Background = blueBrush;
            
        }

        private SolidColorBrush blueBrush = new SolidColorBrush(Colors.LightBlue);
        private SolidColorBrush pinkBrush = new SolidColorBrush(Colors.LightPink);

        private IOneDriveClient oneDriveClient;
        private string text;

        //登陆
        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            prgrsr.IsActive = true;
            #region 登陆
            string[] scopes = new string[] { "wl.signin", "wl.offline_access", "onedrive.readwrite" };
            oneDriveClient = OneDriveClientExtensions.GetClientUsingOnlineIdAuthenticator(scopes);
            await oneDriveClient.AuthenticateAsync();
            #endregion
            text = "Sign In Success!";
            tblID.Text = text;
            prgrsr.IsActive = false;
           


            tblID.Text = text;
            prgrsr.IsActive = false;

            prgrsr.IsActive = false;
        }

        //下载文件
        private async void btnDown_Click(object sender, RoutedEventArgs e)
        {
            prgrsr.IsActive = true;
            #region 得到目标文件
            var item = await oneDriveClient
                     .Drive
                     .Root
                     .ItemWithPath("Documents/你好.txt")
                     .Request()
                     .GetAsync();

            using (var contentStream = await oneDriveClient.Drive.Items[item.Id].Content.Request().GetAsync())
            {
                StreamReader reader = new StreamReader(contentStream);
                text = reader.ReadToEnd();
            }
            #endregion
            tblID.Text = text;
            prgrsr.IsActive = false;
        }

        //上传文件
        private async void btnUpLoad_Click(object sender, RoutedEventArgs e)
        {
            prgrsr.IsActive = true;
            text = "hello there this is an new thinghdjd这是结尾";
            #region 上传目标文件         
            byte[] array = Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(array);
            var uploadedItem = await oneDriveClient
                                           .Drive
                                           .Root
                                           .ItemWithPath("Documents/你好.txt")
                                           .Content
                                           .Request()
                                           .PutAsync<Item>(stream);
            #endregion
            tblID.Text = "上传成功！";
            prgrsr.IsActive = false;

        }

        //本地备份
        private async void btnBac_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker picker = new FileSavePicker();
            picker.DefaultFileExtension = ".pwsv";
            picker.FileTypeChoices.Add("密码计算器备份", new List<string>() { ".pwsv" });
            picker.SuggestedFileName = "backup";
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            StorageFile file = await picker.PickSaveFileAsync();
            text = "fgdashfsdahfjksadjgkhasfiuw的哇大王打算啊打算erfhjksdfhdwadadwadjkdwadasfasdfsdcfsda";
            if (file != null) { 
            await FileIO.WriteTextAsync(file, text);
            }
        }

        //本地还原
        private async void btnDeBac_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openFile = new FileOpenPicker();
            openFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openFile.FileTypeFilter.Add(".pwsv" );
            StorageFile file = await openFile.PickSingleFileAsync();
            text = await FileIO.ReadTextAsync(file);
            tblID.Text = text;
        }

        //存入本地
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder localFolder =ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync("data.pwsv", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, tbx.Text);
        }

        //本地读取
        private async void btnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await localFolder.GetFileAsync("data.pwsv");
                tblID.Text = await FileIO.ReadTextAsync(sampleFile);
            }
            catch(Exception ex)
            {
                tblID.Text = ex.Message;
            }
            
        }

        //roaming写入
        private async void btnRoamingSave_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder RoamingFolder = ApplicationData.Current.RoamingFolder;
            //Debug.WriteLine(ApplicationData.Current.RoamingStorageQuota);
            StorageFile savedFile = await RoamingFolder.CreateFileAsync("data.pwsv", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(savedFile, tbx.Text);
        }

        //roaming读出
        private async void btnRoamingRead_Click(object sender, RoutedEventArgs e)
        {
            StorageFolder RoamingFolder = ApplicationData.Current.RoamingFolder;
            StorageFile savedFile = await RoamingFolder.GetFileAsync("data.pwsv");
            tblID.Text= await FileIO.ReadTextAsync(savedFile);
        }
    }
}
