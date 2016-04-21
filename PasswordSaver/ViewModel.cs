using PasswordSaver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordSaver
{
    public class ViewModel : NotificationObject
    {
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


        public void CheckPassword(string pwd)
        {
            if (pwd == FileManager.GetCode())
                IsCheck = true;
            else
                IsCheck = false;
        }

        public void Go()
        {
            Debug.WriteLine("aaa");
        }

        public ViewModel()
        {
            IsCheck = false;
            RecordItems = new ObservableCollection<RecordItem>();
            RecordItems.Add(new RecordItem("w1", "a1", "p1", "n1"));
            RecordItems.Add(new RecordItem("w2", "a2", "p2", "n2"));
        }
    }
}
