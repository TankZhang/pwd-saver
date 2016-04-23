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
                _isCheck = value;
                if (value)
                    IsListVisible = true;
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

        bool _isUcPwdModifyVisible;
        public bool IsUcPwdModifyVisible
        {
            get
            {
                return _isUcPwdModifyVisible;
            }

            set
            {
                _isUcPwdModifyVisible = value;
                RaisedPropertyChanged("IsUcPwdModifyVisible");
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

        RecordItem _recordItemTemp1;
        public RecordItem RecordItemTemp1
        {
            get
            {
                return _recordItemTemp1;
            }

            set
            {
                _recordItemTemp1 = value;
                RaisedPropertyChanged("RecordItemTemp");
            }
        }

        RecordItem _recordItemTemp2;
        public RecordItem RecordItemTemp2
        {
            get
            {
                return _recordItemTemp2;
            }

            set
            {
                _recordItemTemp2 = value;
                RaisedPropertyChanged("RecordItemTemp2");
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

        ICommand _modifyInCmd;
        public ICommand ModifyInCmd
        {
            get
            {
                return _modifyInCmd;
            }

            set
            {
                _modifyInCmd = value;
            }
        }




        #endregion


        //校验密码，如果对的就更新当前RecordItems
        public async Task CheckPasswordAsync(string pwd)
        {
            if (EncryptHelper.PwdEncrypt(pwd) == RightPwdMd5)
            {
                IsCheck = true;
                RightPwd = pwd;
                if (RecordItems.Count < 1)
                {
                    IsProgressRingVisible = true;
                    string encryptStr = await FileManager.ReadRoamingDataAsync();
                    await ReadRecordAsync();
                    IsProgressRingVisible = false;
                }
            }
            else
                IsCheck = false;
        }

        //更改数据
        public async void ModifyData(object o)
        {
            RecordItem recordItem = (RecordItem)o as RecordItem;
            foreach (RecordItem item in RecordItems)
            {
                if(item.WebSite==RecordItemTemp1.WebSite&& item.Account == RecordItemTemp1.Account)
                {
                    item.WebSite = recordItem.WebSite;
                    item.Account = recordItem.Account;
                    item.Pwd = recordItem.Pwd;
                    item.Note = recordItem.Note;
                    break;
                }
            }
            await SaveRecordAsync();
        }

        //进入更改数据的设置
        public void ModifyIn(object o)
        {
            RecordItem recordItem = (RecordItem)o as RecordItem;
            RecordItemTemp1.WebSite = recordItem.WebSite;
            RecordItemTemp1.Account = recordItem.Account;
            RecordItemTemp1.Pwd = recordItem.Pwd;
            RecordItemTemp1.Note = recordItem.Note;
            RecordItemTemp2.WebSite = recordItem.WebSite;
            RecordItemTemp2.Account = recordItem.Account;
            RecordItemTemp2.Pwd = recordItem.Pwd;
            RecordItemTemp2.Note = recordItem.Note;
            IsUcPwdModifyVisible = true;
            IsGrdPwdsListVisible = false;
        }

        //将当前的RecordItems保存到内存中
        private async Task SaveRecordAsync()
        {
            string jsonStr = FileManager.GetJsonString<ObservableCollection<RecordItem>>(RecordItems);
            string encryptStr = EncryptHelper.DESEncrypt(RightPwd, jsonStr);
            await FileManager.WriteToRoamingDataAsync(encryptStr);
        }

        //将当前内存中的数据读取到RecordItems中
        private async Task<bool> ReadRecordAsync()
        {
            string encryptStr = await FileManager.ReadRoamingDataAsync();
            if (encryptStr != "-1")
            {
                string decryptStr = EncryptHelper.DESDecrypt(RightPwd, encryptStr);
                RecordItems.Clear();
                foreach (RecordItem item in FileManager.ReadFromJson<ObservableCollection<RecordItem>>(decryptStr))
                { RecordItems.Add(item); }
                return true;
            }
            else
            {
                return false;
            }
        }
        //构造函数
        public ViewModel()
        {
            IsCheck = false;
            IsProgressRingVisible = false;
            IsGrdPwdsListVisible = true;
            IsUcPwdModifyVisible = false;
            ModifyCmd = new RelayCommand(new Action<object>(ModifyData));
            ModifyInCmd=new RelayCommand(new Action<object>(ModifyIn));
            RightPwdMd5 = FileManager.GetCode();
            RecordItems = new ObservableCollection<RecordItem>();
            RecordItemTemp1 = new RecordItem();
            RecordItemTemp2 = new RecordItem();
        }
    }
}
