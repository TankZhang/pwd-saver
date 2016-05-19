using PasswordSaver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PasswordSaver
{
    public class ViewModel : NotificationObject
    {
        #region 各类标志位和属性
        private bool _isCheck;
        public bool IsCheck
        {
            get
            {
                return _isCheck;
            }

            set
            {
                if (value)
                    IsListVisible = true;
                _isCheck = value;
                RaisedPropertyChanged("IsCheck");
            }
        }

        private bool _isListVisible;
        public bool IsListVisible
        {
            get
            {
                return _isListVisible;
            }

            set
            {
                _isListVisible = value;
                RaisedPropertyChanged("IsListVisible");
            }
        }

        private bool isGrdListVisible;
        public bool IsGrdListVisible
        {
            get
            {
                return isGrdListVisible;
            }

            set
            {
                isGrdListVisible = value;
                RaisedPropertyChanged("IsGrdListVisible");
            }
        }

        bool _isGrdPwdsListVisible;
        public bool IsGrdPwdsListVisible
        {
            get
            {
                return _isGrdPwdsListVisible;
            }

            set
            {
                _isGrdPwdsListVisible = value;
                RaisedPropertyChanged("IsGrdPwdsListVisible");
            }
        }

        bool _isUcItemDetailVisible;
        public bool IsUcItemDetailVisible
        {
            get
            {
                return _isUcItemDetailVisible;
            }

            set
            {
                _isUcItemDetailVisible = value;
                RaisedPropertyChanged("IsUcItemDetailVisible");
            }
        }

        private string _userInputPwd;
        public string UserInputPwd
        {
            get
            {
                return _userInputPwd;
            }

            set
            {
                _userInputPwd = value;
                RaisedPropertyChanged("UserInputPwd");
            }
        }

        private string _rightPwd;
        public string RightPwd
        {
            get
            {
                return _rightPwd;
            }

            set
            {
                _rightPwd = value;
            }
        }

        private string _rightPwdMd5;
        public string RightPwdMd5
        {
            get
            {
                return _rightPwdMd5;
            }

            set
            {
                _rightPwdMd5 = value;
            }
        }

        private bool _isProgressRingVisible;
        public bool IsProgressRingVisible
        {
            get
            {
                return _isProgressRingVisible;
            }

            set
            {
                _isProgressRingVisible = value;
                RaisedPropertyChanged("IsProgressRingVisible");
            }
        }

        ObservableCollection<RecordItem> _recordItems;
        public ObservableCollection<RecordItem> RecordItems
        {
            get
            {
                return _recordItems;
            }

            set
            {
                _recordItems = value;
                RaisedPropertyChanged("RecordItems");
            }
        }

        RecordItem _recordItemMemory;
        public RecordItem RecordItemMemory
        {
            get
            {
                return _recordItemMemory;
            }

            set
            {
                _recordItemMemory = value;
                RaisedPropertyChanged("RecordItemMemory");
            }
        }

        RecordItem _recordItemToModify;
        public RecordItem RecordItemToModify
        {
            get
            {
                return _recordItemToModify;
            }

            set
            {
                _recordItemToModify = value;
                RaisedPropertyChanged("RecordItemToModify");
            }
        }

        //Modify为true
        bool _isModifyOrAdd;
        public bool IsModifyOrAdd
        {
            get
            {
                return _isModifyOrAdd;
            }

            set
            {
                _isModifyOrAdd = value;
                RaisedPropertyChanged("IsModifyOrAdd");
            }
        }

        bool _isBackVisible;
        public bool IsBackVisible
        {
            get
            {
                return _isBackVisible;
            }

            set
            {
                _isBackVisible = value;
                RaisedPropertyChanged("IsBackVisible");
            }
        }


        string _title;
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                RaisedPropertyChanged("Title");
            }
        }

        string _settingResult;
        public string SettingResult
        {
            get
            {
                return _settingResult;
            }

            set
            {
                _settingResult = value;
                RaisedPropertyChanged("SettingResult");
            }
        }

        bool _isLstMainSelected;
        public bool IsLstMainSelected
        {
            get
            {
                return _isLstMainSelected;
            }

            set
            {
                _isLstMainSelected = value;
                RaisedPropertyChanged("IsLstMainSelected");
            }
        }
        #endregion

        #region 各类命令
        ICommand _modifyCmd;
        public ICommand ModifyCmd
        {
            get
            {
                return _modifyCmd;
            }

            set
            {
                _modifyCmd = value;
            }
        }

        ICommand _backCmd;
        public ICommand BackCmd
        {
            get
            {
                return _backCmd;
            }

            set
            {
                _backCmd = value;
                RaisedPropertyChanged("BackCmd");
            }
        }

        ICommand _addCmd;
        public ICommand AddCmd
        {
            get
            {
                return _addCmd;
            }

            set
            {
                _addCmd = value;
                RaisedPropertyChanged("AddCmd");
            }
        }


        #endregion


        //校验密码，如果对的就更新当前RecordItems
        public async Task<bool> CheckPasswordAsync(string pwd)
        {
            if (EncryptHelper.PwdEncrypt(pwd) == RightPwdMd5)
            {
                IsCheck = true;
                RightPwd = pwd;
                //如果现有列表中没有数据，即打开应用的第一次验证
                if (RecordItems.Count < 1)
                {
                    IsProgressRingVisible = true;
                    string str = await FileManager.RecoverAsync(SaveType.LocalState);
                    if (!str.StartsWith("-"))
                    {
                        str = str.Substring(33);
                        string exStr = DecodeRecord(str);
                        if (exStr.StartsWith("-"))
                            Title = exStr.Substring(1);
                    }
                    IsProgressRingVisible = false;
                }
            }
            else
                IsCheck = false;
            return IsCheck;
        }

        //更改数据,找到与记忆条目相同的，更改之，然后返回去
        public async void ModifyData()
        {
            int index = FindIndexOf(RecordItemMemory.WebSite, RecordItemMemory.Account);
            CopyRecordItem(RecordItemToModify, RecordItems[index]);
            string str = await SaveRecordAsync(SaveType.LocalState);
            if (str.StartsWith("-"))
                Title = str.Substring(1);
            else
                Title = "收藏列表";
            IsUcItemDetailVisible = false;
            IsGrdPwdsListVisible = true;
            IsBackVisible = false;
        }

        //进入更改数据的设置,RecordItemMemory为记忆条目，RecordItemToModify为被绑定待修改条目
        public void GoToModify(RecordItem recordItem)
        {
            //RecordItem recordItem = (RecordItem)o as RecordItem;
            CopyRecordItem(recordItem, RecordItemMemory);
            CopyRecordItem(recordItem, RecordItemToModify);
            IsUcItemDetailVisible = true;
            IsGrdPwdsListVisible = false;
            IsModifyOrAdd = true;
            IsBackVisible = true;
            Title = "修改条目";
        }

        //进入添加新条目，清空RecordItemToModify，然后让IsUcItemDetailVisible为true
        public void GoToAdd()
        {
            RecordItemToModify = new RecordItem();
            IsUcItemDetailVisible = true;
            IsModifyOrAdd = false;
        }

        //添加新条目，如果没有相同网站和相同账号，添加之，并保存，并显示列表。
        public async void AddData()
        {
            if (string.IsNullOrEmpty(RecordItemToModify.WebSite))
            {
                RecordItemToModify.WebSite = "错误！网站名称不能为空";
                return;
            }
            int index = FindIndexOf(RecordItemToModify.WebSite, RecordItemToModify.Account);
            if (index > -1)
            {
                RecordItemToModify.WebSite = "错误！已存在当前网站";
                RecordItemToModify.Account = "错误！已存在当前账户";
            }
            else
            {
                RecordItem r = new RecordItem();
                CopyRecordItem(RecordItemToModify, r);
                RecordItems.Add(r);
                string titleStr = await SaveRecordAsync(SaveType.LocalState);
                if (titleStr.StartsWith("-"))
                    Title = titleStr.Substring(1);
                else
                    Title = "收藏列表";
                IsUcItemDetailVisible = false;
                IsGrdPwdsListVisible = true;
                IsBackVisible = false;
                IsListVisible = true;
            }
        }

        //删除条目，
        public async void DeleteData(RecordItem recordItem)
        {
            int index = FindIndexOf(recordItem.WebSite, recordItem.Account);
            RecordItems.RemoveAt(index);
            await SaveRecordAsync(SaveType.LocalState);
        }

        //返回函数
        public void Back()
        {
            IsUcItemDetailVisible = false;
            IsGrdPwdsListVisible = true;
            IsBackVisible = false;
            Title = "收藏列表";
        }

        //修改密码，修改当前密码，然后将修改后的数据存入本地文件夹
        public async void ChangePwd(string pwd)
        {
            string pwdMd5 = EncryptHelper.PwdEncrypt(pwd);
            RightPwd = pwd;
            RightPwdMd5 = pwdMd5;
            string codeStr = CodeRecord();
            string strToSave = pwdMd5 + "|" + codeStr;
            string result = await FileManager.BackupAsync(strToSave, SaveType.LocalState);
            if (result.StartsWith("-"))
                SettingResult = result.Substring(1);
            else { SettingResult = "修改成功！"; }
        }

        //备份函数
        public async void BackupAsync(SaveType st)
        {
            IsProgressRingVisible = true;
            SettingResult = "开始备份...";
            string codeStr = CodeRecord();
            string strToSave = RightPwdMd5 + "|" + codeStr;
            string strResult = await FileManager.BackupAsync(strToSave, st);
            if (strResult.StartsWith("-"))
                SettingResult = strResult.Substring(1);
            else
                SettingResult = "备份成功！";
            IsProgressRingVisible = false;
        }

        //读出备份数据，同时将备份数据写入本地文件
        public async void ReadBackupAsync(SaveType st)
        {
            IsProgressRingVisible = true;
            SettingResult = "开始恢复...";
            string strRecover = await FileManager.RecoverAsync(st);
            if (strRecover.StartsWith("-"))
            {
                SettingResult = strRecover.Substring(1);
                IsProgressRingVisible = false;
                return;
            }
            RightPwdMd5 = strRecover.Substring(0, 32);
            string codeStr = strRecover.Substring(33);
            string strResult = DecodeRecord(codeStr);
            if (strResult.StartsWith("-"))
                SettingResult = strResult.Substring(1);
            else
            {
                SettingResult = "恢复成功！";
                await SaveRecordAsync(SaveType.LocalState);
            }
            IsProgressRingVisible = false;
        }

        //复制条目
        private void CopyRecordItem(RecordItem copyFrom, RecordItem copyTo)
        {
            copyTo.WebSite = copyFrom.WebSite;
            copyTo.Account = copyFrom.Account;
            copyTo.Pwd = copyFrom.Pwd;
            copyTo.Note = copyFrom.Note;
        }

        //找到条目列表中的某个数据的下标
        private int FindIndexOf(string website, string account)
        {
            for (int i = 0; i < RecordItems.Count; i++)
            {
                if (RecordItems[i].WebSite == website && RecordItems[i].Account == account)
                    return i;
            }
            return -1;
        }

        //将当前的数据全部保存
        private async Task<string> SaveRecordAsync(SaveType st)
        {
            IsProgressRingVisible = true;
            string codeStr = CodeRecord();
            string strToSave = RightPwdMd5 + "|" + codeStr;
            string returnStr = await FileManager.BackupAsync(strToSave, st);
            IsProgressRingVisible = false;
            return returnStr;
        }

        //将当前的record生成新的str
        private string CodeRecord()
        {
            string jsonStr = FileManager.GetJsonString<ObservableCollection<RecordItem>>(RecordItems);
            return EncryptHelper.DESEncrypt(RightPwd, jsonStr);
        }

        //将当前str读取到RecordItems中
        private string DecodeRecord(string str)
        {
            try
            {
                string decryptStr = EncryptHelper.DESDecrypt(RightPwd, str);
                RecordItems.Clear();
                foreach (RecordItem item in FileManager.ReadFromJson<ObservableCollection<RecordItem>>(decryptStr))
                { RecordItems.Add(item); }
                IsProgressRingVisible = false;
                return "1";
            }
            catch (Exception ex) { return "-" + ex.Message; }
        }

        //初始化函数
        private async void VMInit()
        {
            IsProgressRingVisible = true;
            IsCheck = false;
            IsGrdPwdsListVisible = false;
            IsUcItemDetailVisible = false;
            ModifyCmd = new RelayCommand(new Action(ModifyData));
            BackCmd = new RelayCommand(new Action(Back));
            AddCmd = new RelayCommand(new Action(AddData));
            string str = await FileManager.RecoverAsync(SaveType.LocalState);
            if (str.StartsWith("-"))
            { RightPwdMd5 = EncryptHelper.PwdEncrypt("123"); }
            else
            { RightPwdMd5 = FileManager.GetCode(str); }
            RecordItems = new ObservableCollection<RecordItem>();
            RecordItemMemory = new RecordItem();
            RecordItemToModify = new RecordItem();
            IsBackVisible = false;
            Title = "主页";
            IsProgressRingVisible = false;

        }

        //构造函数
        public ViewModel()
        {
            VMInit();
        }
    }
}
