using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ViewModel VM = new ViewModel();

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = VM;
            grdMain.Background = blueBrush;
            
        }

        private SolidColorBrush blueBrush = new SolidColorBrush(Colors.LightBlue);
        private SolidColorBrush pinkBrush = new SolidColorBrush(Colors.LightPink);

        private IOneDriveClient oneDriveClient;
        private string text;

        //备份测试 2016年5月24日20:40:17
        /*
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
        */


        //SemanticZoom测试 2016年5月24日20:40:34

        /// <summary> 
        /// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母 http://www.cnblogs.com/glacierh/archive/2008/08/25/1276113.html
        /// </summary> 
        /// <param name="cnChar">单个汉字</param> 
        /// <returns>单个大写字母</returns> 
        private static string GetCharSpellCode(string cnChar)
        {
            long iCnChar;
            
            byte[] ZW = System.Text.Encoding.Unicode.GetBytes(cnChar);


            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            ZW = gb2312.GetBytes(cnChar);
            //gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            ////返回转换后的字符   
            //return utf8.GetString(gb);



            //如果是字母，则直接返回 
            if (ZW.Length == 1)
            {
                return cnChar.ToUpper();
            }
            else
            {
                // get the array of byte from the single char 
                int i1 = (short)(ZW[0]);
                int i2 = (short)(ZW[1]);
                iCnChar = i1 * 256 + i2;
            }

            //expresstion 
            //table of the constant list 
            // 'A'; //45217..45252 
            // 'B'; //45253..45760 
            // 'C'; //45761..46317 
            // 'D'; //46318..46825 
            // 'E'; //46826..47009 
            // 'F'; //47010..47296 
            // 'G'; //47297..47613 

            // 'H'; //47614..48118 
            // 'J'; //48119..49061 
            // 'K'; //49062..49323 
            // 'L'; //49324..49895 
            // 'M'; //49896..50370 
            // 'N'; //50371..50613 
            // 'O'; //50614..50621 
            // 'P'; //50622..50905 
            // 'Q'; //50906..51386 

            // 'R'; //51387..51445 
            // 'S'; //51446..52217 
            // 'T'; //52218..52697 
            //没有U,V 
            // 'W'; //52698..52979 
            // 'X'; //52980..53640 
            // 'Y'; //53689..54480 
            // 'Z'; //54481..55289 

            // iCnChar match the constant 
            if ((iCnChar >= 45217) && (iCnChar <= 45252))
            {
                return "A";
            }
            else if ((iCnChar >= 45253) && (iCnChar <= 45760))
            {
                return "B";
            }
            else if ((iCnChar >= 45761) && (iCnChar <= 46317))
            {
                return "C";
            }
            else if ((iCnChar >= 46318) && (iCnChar <= 46825))
            {
                return "D";
            }
            else if ((iCnChar >= 46826) && (iCnChar <= 47009))
            {
                return "E";
            }
            else if ((iCnChar >= 47010) && (iCnChar <= 47296))
            {
                return "F";
            }
            else if ((iCnChar >= 47297) && (iCnChar <= 47613))
            {
                return "G";
            }
            else if ((iCnChar >= 47614) && (iCnChar <= 48118))
            {
                return "H";
            }
            else if ((iCnChar >= 48119) && (iCnChar <= 49061))
            {
                return "J";
            }
            else if ((iCnChar >= 49062) && (iCnChar <= 49323))
            {
                return "K";
            }
            else if ((iCnChar >= 49324) && (iCnChar <= 49895))
            {
                return "L";
            }
            else if ((iCnChar >= 49896) && (iCnChar <= 50370))
            {
                return "M";
            }

            else if ((iCnChar >= 50371) && (iCnChar <= 50613))
            {
                return "N";
            }
            else if ((iCnChar >= 50614) && (iCnChar <= 50621))
            {
                return "O";
            }
            else if ((iCnChar >= 50622) && (iCnChar <= 50905))
            {
                return "P";
            }
            else if ((iCnChar >= 50906) && (iCnChar <= 51386))
            {
                return "Q";
            }
            else if ((iCnChar >= 51387) && (iCnChar <= 51445))
            {
                return "R";
            }
            else if ((iCnChar >= 51446) && (iCnChar <= 52217))
            {
                return "S";
            }
            else if ((iCnChar >= 52218) && (iCnChar <= 52697))
            {
                return "T";
            }
            else if ((iCnChar >= 52698) && (iCnChar <= 52979))
            {
                return "W";
            }
            else if ((iCnChar >= 52980) && (iCnChar <= 53640))
            {
                return "X";
            }
            else if ((iCnChar >= 53689) && (iCnChar <= 54480))
            {
                return "Y";
            }
            else if ((iCnChar >= 54481) && (iCnChar <= 55289))
            {
                return "Z";
            }
            else return ("?");
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = tbxInput.Text;
            str = ChineseHelper.GetFirstWord(str);
            tbxOutput.Text = str;
        }
    }

}
