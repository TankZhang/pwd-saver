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

        bool _isModifyOrAdd;
        //Modify为true
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

        //更改数据,找到与记忆条目相同的，更改之，然后返回去
        public async void ModifyData()
        {
            foreach (RecordItem item in RecordItems)
            {
                if(item.WebSite==RecordItemMemory.WebSite&& item.Account == RecordItemMemory.Account)
                {
                    item.WebSite = RecordItemToModify.WebSite;
                    item.Account = RecordItemToModify.Account;
                    item.Pwd = RecordItemToModify.Pwd;
                    item.Note = RecordItemToModify.Note;
                    break;
                }
            }
            await SaveRecordAsync();
            IsUcItemDetailVisible = false;
            IsGrdPwdsListVisible = true;
            IsBackVisible = false;
            Title = "收藏列表";
        }

        //进入更改数据的设置,RecordItemMemory为记忆条目，RecordItemToModify为被绑定待修改条目
        public void ModifyIn(object o)
        {
            RecordItem recordItem = (RecordItem)o as RecordItem;
            RecordItemMemory.WebSite = recordItem.WebSite;
            RecordItemMemory.Account = recordItem.Account;
            RecordItemMemory.Pwd = recordItem.Pwd;
            RecordItemMemory.Note = recordItem.Note;
            RecordItemToModify.WebSite = recordItem.WebSite;
            RecordItemToModify.Account = recordItem.Account;
            RecordItemToModify.Pwd = recordItem.Pwd;
            RecordItemToModify.Note = recordItem.Note;
            IsUcItemDetailVisible = true;
            IsGrdPwdsListVisible = false;
            IsModifyOrAdd = true;
            IsBackVisible = true;
            Title = "修改条目";
        }

        //返回函数
        public void Back()
        {
            IsUcItemDetailVisible = false;
            IsGrdPwdsListVisible = true;
            IsBackVisible = false;
            Title = "收藏列表";
            Debug.WriteLine("back!!!");
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
            IsGrdPwdsListVisible = false;
            IsUcItemDetailVisible = false;
            ModifyCmd = new RelayCommand(new Action(ModifyData));
            ModifyInCmd=new RelayCommand(new Action<object>(ModifyIn));
            BackCmd = new RelayCommand(new Action(Back));
            RightPwdMd5 = FileManager.GetCode();
            RecordItems = new ObservableCollection<RecordItem>();
            RecordItemMemory = new RecordItem();
            RecordItemToModify = new RecordItem();
            IsBackVisible = false;
            Title = "主页";
        }
    }
}
