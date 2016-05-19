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
            if(Regex.IsMatch(pwbxNewPwd.Password,regexStr))
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
        
        private void btnRecoverBackup_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.ReadBackupAsync(SaveType.LocalFile);
        }

        private void btnStartBackup_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.BackupAsync(SaveType.LocalFile);
        }
        
        private void btnStartOneDriveBackup_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.BackupAsync(SaveType.OneDrive);
        }

        private void btnRecoverOneDriveBackup_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.ReadBackupAsync(SaveType.OneDrive);
        }

        private void btnStartRoamingBackup_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.BackupAsync(SaveType.RoamingData);

        }

        private void btnRecoverRoamingBackup_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.ReadBackupAsync(SaveType.RoamingData);

        }

        private void ListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lstbiGoChange.IsSelected)
            {
                lstbiChangePwd.Visibility = lstbiChangePwd.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                lstbiOneDriveBackup.Visibility = Visibility.Collapsed;
                lstbiRoamingBackup.Visibility = Visibility.Collapsed;
                lstbiLocalBackup.Visibility = Visibility.Collapsed;
            }
            if (lstbiGoOneDriveBackup.IsSelected)
            {
                lstbiChangePwd.Visibility = Visibility.Collapsed;
                lstbiOneDriveBackup.Visibility = lstbiOneDriveBackup.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                lstbiRoamingBackup.Visibility = Visibility.Collapsed;
                lstbiLocalBackup.Visibility = Visibility.Collapsed;
            }
            if (lstbiGoLocalBackup.IsSelected)
            {
                lstbiChangePwd.Visibility = Visibility.Collapsed;
                lstbiOneDriveBackup.Visibility = Visibility.Collapsed;
                lstbiRoamingBackup.Visibility = Visibility.Collapsed;
                lstbiLocalBackup.Visibility = lstbiLocalBackup.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
            if (lstbiGoRoamingBackup.IsSelected)
            {
                lstbiChangePwd.Visibility = Visibility.Collapsed;
                lstbiOneDriveBackup.Visibility = Visibility.Collapsed;
                lstbiRoamingBackup.Visibility = lstbiRoamingBackup.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                lstbiLocalBackup.Visibility = Visibility.Collapsed;
            }

        }
    }
}
