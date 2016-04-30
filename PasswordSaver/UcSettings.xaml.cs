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
    public sealed partial class UcSettings : UserControl
    {
        public UcSettings()
        {
            this.InitializeComponent();
        }

        private void btnChangePwd_Click(object sender, RoutedEventArgs e)
        {
            stkpBackup.Visibility = Visibility.Collapsed;
            if (stkpChangePwd.Visibility == Visibility.Visible)
                stkpChangePwd.Visibility = Visibility.Collapsed;
            else
                stkpChangePwd.Visibility = Visibility.Visible;
        }

        private void btnChangePwdConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (pwbxNewPwd.Password != pwbxNewPwdConfirm.Password)
            {
                tbxOldPwd.Text = "错误！新密码不一致";
                return;
            }
            if (string.IsNullOrEmpty(pwbxNewPwd.Password))
            {
                tbxOldPwd.Text = "错误！新密码不能为空";
                return;
            }
            ViewModel vm = (ViewModel)this.DataContext;
            if (EncryptHelper.PwdEncrypt(tbxOldPwd.Text) != vm.RightPwdMd5)
            {
                tbxOldPwd.Text = "错误！密码错误";
                return;
            }
            vm.ChangePwd(pwbxNewPwd.Password);
            tbxOldPwd.Text = "修改成功！";

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

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            stkpChangePwd.Visibility = Visibility.Collapsed;
            if (stkpBackup.Visibility == Visibility.Visible)
                stkpBackup.Visibility = Visibility.Collapsed;
            else
                stkpBackup.Visibility = Visibility.Visible;

        }

        private void btnRecoverBackup_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.ReadBackupAsync();
        }

        private void btnStartBackup_Click(object sender, RoutedEventArgs e)
        {
            ViewModel vm = (ViewModel)this.DataContext;
            vm.BackupAsync();
        }
    }
}
