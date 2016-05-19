using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class UcSettings : UserControl
    {
        public UcSettings()
        {
            this.InitializeComponent();
            txbDoc.Text = "1、为什么OneDrive存储总是等待好长时间后报错？\n    建议检查当前设备能否正常访问OneDrive。\n" +
                "2、Roaming恢复备份迟迟不同步？\n    Roaming机制有本身的问题，频繁同步或者网络环境稍差都会影响同步成功率和同步速度，并且文件大小限制为100kb（正常使用大约为100组数据），所以用户体验并没预计的好。\n" +
                "3、恢复备份后密码不能用了？\n    由于备份数据是通过密码加密的，所以恢复备份后请使用备份数据时候的软件登陆密码进行操作。\n";
        }

        private void btnChangePwdConfirm_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            if (pwbxNewPwd.Password != pwbxNewPwdConfirm.Password)
            {
                vm.SettingResult = "错误！新密码不一致";
                return;
            }
            if (string.IsNullOrEmpty(pwbxNewPwd.Password))
            {
                vm.SettingResult = "错误！新密码不能为空";
                return;
            }
            string regexStr = @"\D";
            if (Regex.IsMatch(pwbxNewPwd.Password, regexStr))
            {
                vm.SettingResult = "错误！新密码只能为数字";
                return;
            }
            if (EncryptHelper.PwdEncrypt(tbxOldPwd.Text) != vm.RightPwdMd5)
            {
                vm.SettingResult = "错误！密码错误";
                return;
            }
            vm.ChangePwd(pwbxNewPwd.Password);

        }

        private void tbxOldPwd_GotFocus(object sender, RoutedEventArgs e)
        {
            tbxOldPwd.Text = "";
        }

        private void pwbxNewPwd_GotFocus(object sender, RoutedEventArgs e)
        {
            pwbxNewPwd.Password = "";
        }

        private void pwbxNewPwdConfirm_GotFocus(object sender, RoutedEventArgs e)
        {
            pwbxNewPwdConfirm.Password = "";
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            Button btn = (Button)sender;
            switch (btn.Name)
            {
                case "btnRecoverBackup":
                    vm.ReadBackupAsync(SaveType.LocalFile);
                    break;
                case "btnStartBackup":
                    vm.BackupAsync(SaveType.LocalFile);
                    break;
                case "btnStartOneDriveBackup":
                    vm.BackupAsync(SaveType.OneDrive);
                    break;
                case "btnRecoverOneDriveBackup":
                    vm.ReadBackupAsync(SaveType.OneDrive);
                    break;
                case "btnStartRoamingBackup":
                    vm.BackupAsync(SaveType.RoamingData);
                    break;
                case "btnRecoverRoamingBackup":
                    vm.ReadBackupAsync(SaveType.RoamingData);
                    break;
                case "btnExport":
                    vm.ExportData();
                    break;
                default:
                    vm.SettingResult = "遇到未知请求";
                    break;
            }
        }

        private void ListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lstbiGoChange.IsSelected)
            {
                lstbiChangePwd.Visibility = lstbiChangePwd.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                lstbiOneDriveBackup.Visibility = Visibility.Collapsed;
                lstbiRoamingBackup.Visibility = Visibility.Collapsed;
                lstbiLocalBackup.Visibility = Visibility.Collapsed;
                lstbiDoc.Visibility = Visibility.Collapsed;
                lstbiExport.Visibility = Visibility.Collapsed;
            }
            if (lstbiGoOneDriveBackup.IsSelected)
            {
                lstbiChangePwd.Visibility = Visibility.Collapsed;
                lstbiOneDriveBackup.Visibility = lstbiOneDriveBackup.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                lstbiRoamingBackup.Visibility = Visibility.Collapsed;
                lstbiLocalBackup.Visibility = Visibility.Collapsed;
                lstbiDoc.Visibility = Visibility.Collapsed;
                lstbiExport.Visibility = Visibility.Collapsed;
            }
            if (lstbiGoLocalBackup.IsSelected)
            {
                lstbiChangePwd.Visibility = Visibility.Collapsed;
                lstbiOneDriveBackup.Visibility = Visibility.Collapsed;
                lstbiRoamingBackup.Visibility = Visibility.Collapsed;
                lstbiLocalBackup.Visibility = lstbiLocalBackup.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                lstbiDoc.Visibility = Visibility.Collapsed;
                lstbiExport.Visibility = Visibility.Collapsed;
            }
            if (lstbiGoRoamingBackup.IsSelected)
            {
                lstbiChangePwd.Visibility = Visibility.Collapsed;
                lstbiOneDriveBackup.Visibility = Visibility.Collapsed;
                lstbiRoamingBackup.Visibility = lstbiRoamingBackup.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                lstbiLocalBackup.Visibility = Visibility.Collapsed;
                lstbiDoc.Visibility = Visibility.Collapsed;
                lstbiExport.Visibility = Visibility.Collapsed;
            }
            if (lstbiGoDoc.IsSelected)
            {
                lstbiChangePwd.Visibility = Visibility.Collapsed;
                lstbiOneDriveBackup.Visibility = Visibility.Collapsed;
                lstbiRoamingBackup.Visibility = Visibility.Collapsed;
                lstbiDoc.Visibility = lstbiDoc.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                lstbiLocalBackup.Visibility = Visibility.Collapsed;
                lstbiExport.Visibility = Visibility.Collapsed;
            }
            if(lstbiGoExport.IsSelected)
            {
                lstbiChangePwd.Visibility = Visibility.Collapsed;
                lstbiOneDriveBackup.Visibility = Visibility.Collapsed;
                lstbiRoamingBackup.Visibility = Visibility.Collapsed;
                lstbiExport.Visibility = lstbiExport.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                lstbiLocalBackup.Visibility = Visibility.Collapsed;
                lstbiDoc.Visibility = Visibility.Collapsed;

            }

        }
        
    }
}
