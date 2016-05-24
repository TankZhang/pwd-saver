using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RoamingTest
{
    [DataContract]
    public class RecordItem : NotificationObject
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



        public RecordItem(string website, string accout, string pwd)
        {
            WebSite = website;
            Account = accout;
            Pwd = pwd;
        }
        public RecordItem()
        {
        }
    }
}
