using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPTest
{
    public class TViewModel : NotificationObject
    {
        public ObservableCollection<AlphaKeyGroup<RecordItem>> Records { get; set; }
        public TViewModel()
        {
            Records = new ObservableCollection<AlphaKeyGroup<RecordItem>>();
            LoadData();
        }

        public void LoadData()
        {
            List<RecordItem> rList = new List<RecordItem>();
            rList.Add(new RecordItem("F", "z1", "m1", "b1"));
            rList.Add(new RecordItem("W", "z2", "m2", "b2"));
            rList.Add(new RecordItem("CV", "z3", "m3", "b3"));
            rList.Add(new RecordItem("GF", "z4", "m4", "b4"));
            List<AlphaKeyGroup<RecordItem>> groupData = AlphaKeyGroup<RecordItem>.CreateGroups(rList, (RecordItem r) => r.WebSite, true);
            foreach (var item in groupData)
            {
                Records.Add(item);
            }
        }

        public void LoadData1()
        {
            List<RecordItem> rList = new List<RecordItem>();
            rList.Add(new RecordItem("F", "z1", "m1", "b1"));
            rList.Add(new RecordItem("W", "z2", "m2", "b2"));
            rList.Add(new RecordItem("CV", "z3", "m3", "b3"));
            rList.Add(new RecordItem("GF", "z4", "m4", "b4"));
            rList.Add(new RecordItem("F", "z1", "m1", "b1"));
            rList.Add(new RecordItem("W", "z2", "m2", "b2"));
            rList.Add(new RecordItem("CV", "z3", "m3", "b3"));
            rList.Add(new RecordItem("GF", "z4", "m4", "b4"));
            rList.Add(new RecordItem("F", "z1", "m1", "b1"));
            rList.Add(new RecordItem("W", "z2", "m2", "b2"));
            rList.Add(new RecordItem("CV", "z3", "m3", "b3"));
            rList.Add(new RecordItem("GF", "z4", "m4", "b4"));
            Records.Clear();
            List<AlphaKeyGroup<RecordItem>> groupData = AlphaKeyGroup<RecordItem>.CreateGroups(rList, (RecordItem r) => r.WebSite, true);
            foreach (var item in groupData)
            {
                Records.Add(item);
            }
        }
    }

}
