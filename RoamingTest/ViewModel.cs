using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoamingTest
{
   public class ViewModel: NotificationObject
    {
        ObservableCollection<AlphaKeyGroup<RecordItem>> _records;
        // ObservableCollection<AlphaKeyGroup<RecordItem>> Records { get; set; }

        public ViewModel()
        {
            Records = new ObservableCollection<AlphaKeyGroup<RecordItem>>();
            Load();
        }

        public ObservableCollection<AlphaKeyGroup<RecordItem>> Records
        {
            get
            {
                return _records;
            }

            set
            {
                _records = value;
                RaisedPropertyChanged("Records");
            }
        }

        public void Load()
        {
            List<RecordItem> rList = new List<RecordItem>();
            rList.Add(new RecordItem("网站1","zhangha1", "mim1"));
            rList.Add(new RecordItem("大站1","zhangha好人1", "m大是im1"));
            rList.Add(new RecordItem("好人站1", "zhang倒萨ha1", "m倒萨发im1"));
            rList.Add(new RecordItem("放入站1", "zhan大gha1", "m范德萨im1"));
            rList.Add(new RecordItem("南瓜站1", "zhan是倒萨gha1", "m范德萨发im1"));
            rList.Add(new RecordItem("南瓜站1", "zhan是倒萨gha1", "m范德萨发im1"));

            List<AlphaKeyGroup<RecordItem>> groupData = AlphaKeyGroup<RecordItem>.CreateGroups(rList, (RecordItem r) => r.WebSite, true);
            foreach (var item in groupData)
            {
                Records.Add(item);
            }
        }
    }
}
