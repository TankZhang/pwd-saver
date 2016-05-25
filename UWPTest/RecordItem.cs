using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UWPTest
{
    [DataContract]
    public class RecordItem:NotificationObject
    {
        string _webSite;
        [DataMember]
        public string WebSite
        {
            get
            {
                return _webSite;
            }

            set
            {
                _webSite = value;
                RaisedPropertyChanged("WebSite");
            }
        }

        string _account;
        [DataMember]
        public string Account
        {
            get
            {
                return _account;
            }

            set
            {
                _account = value;
                RaisedPropertyChanged("Account");
            }
        }


        string _pwd;
        [DataMember]
        public string Pwd
        {
            get
            {
                return _pwd;
            }

            set
            {
                _pwd = value;
                RaisedPropertyChanged("Pwd");
            }
        }

        string _note;
        [DataMember]
        public string Note
        {
            get
            {
                return _note;
            }

            set
            {
                _note = value;
                RaisedPropertyChanged("Note");
            }
        }

        public RecordItem(string website, string accout, string pwd, string note)
        {
            WebSite = website;
            Account = accout;
            Pwd = pwd;
            Note = note;
        }
        public RecordItem()
        {
        }
    }
}
