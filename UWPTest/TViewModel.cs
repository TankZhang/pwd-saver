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
        ObservableCollection<AlphaKeyGroup<RecordItem>> _records;
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

        List<RecordItem> _recordList;
        public List<RecordItem> RecordList
        {
            get
            {
                return _recordList;
            }

            set
            {
                _recordList = value;
            }
        }


        public TViewModel()
        {
            Records = new ObservableCollection<AlphaKeyGroup<RecordItem>>();
            RecordList = new List<RecordItem>();
            LoadData();
        }

        public void GoToModify(RecordItem recordItem)
        {
            string website = recordItem.WebSite;
            string account = recordItem.Account;
            int index = RecordList.FindIndex(s => s.WebSite == website && s.Account == account);
            RecordList.RemoveAt(index);
            Records.Clear();
            List<AlphaKeyGroup<RecordItem>> groupData = AlphaKeyGroup<RecordItem>.CreateGroups(RecordList, (RecordItem r) => r.WebSite, true);
            foreach (var item in groupData)
            {
                Records.Add(item);
            }
        }

        public void LoadData()
        {
            RecordList.Add(new RecordItem("那撒旦W", "z2", "m2", "b2"));
            RecordList.Add(new RecordItem("呢撒旦W", "z2", "m2", "b2"));
            RecordList.Add(new RecordItem("被撒旦W", "z2", "m2", "b2"));
            RecordList.Add(new RecordItem("吧撒旦W", "z2", "m2", "b2"));
            RecordList.Add(new RecordItem("剥fasf 阿斯顿F", "z1", "m1", "b1"));
            RecordList.Add(new RecordItem("的撒旦W", "z2", "m2", "b2"));
            RecordList.Add(new RecordItem("倒萨CV", "z3", "m3", "b3"));
            RecordList.Add(new RecordItem("大大撒GF", "z4", "m4", "b4"));
            RecordList.Add(new RecordItem("倒萨CV", "z3", "m3", "b3"));
            RecordList.Add(new RecordItem("大大撒GF", "z4", "m4", "b4"));
            RecordList.Add(new RecordItem("对大撒GF", "z4", "m4", "b4"));
            RecordList.Add(new RecordItem("s倒萨CV", "z3", "m3", "b3"));
            RecordList.Add(new RecordItem("大大撒GF", "z4", "m4", "b4"));
            RecordList.Add(new RecordItem("23大大撒GF", "z4", "m4", "b4"));
            RecordList.Add(new RecordItem(".倒萨CV", "z3", "m3", "b3"));
            RecordList.Add(new RecordItem("大大撒GF", "z4", "m4", "b4"));
            RecordList.Add(new RecordItem("大大撒GF", "z4", "m4", "b4"));
            RecordList.Add(new RecordItem("倒萨CV", "z3", "m3", "b3"));
            RecordList.Add(new RecordItem("大大撒GF", "z4", "m4", "b4"));
            List<AlphaKeyGroup<RecordItem>> groupData = AlphaKeyGroup<RecordItem>.CreateGroups(RecordList, (RecordItem r) => r.WebSite, true);
            foreach (var item in groupData)
            {
                Records.Add(item);
            }
        }

        public void LoadData1()
        {
            Random ra = new Random();
            string s = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            List<RecordItem> rList = new List<RecordItem>();
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z1很舒服的后果犯得上广泛大锅饭时代公司发的", "m1", "b1"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z2很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m2", "b2"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z3很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m3", "b3"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z4很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m4", "b4"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z1很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m1", "b1"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z2很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m2", "b2"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z3很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m3", "b3"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z4很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m4", "b4"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z1很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m1", "b1"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z2很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m2", "b2"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z3很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m3", "b3"));
            rList.Add(new RecordItem(s[ra.Next(25)].ToString(), "z4很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的很舒服的后果犯得上广泛大锅饭时代公司发的", "m4", "b4"));
            Records.Clear();
            List<AlphaKeyGroup<RecordItem>> groupData = AlphaKeyGroup<RecordItem>.CreateGroups(rList, (RecordItem r) => r.WebSite, true);
            foreach (var item in groupData)
            {
                Records.Add(item);
            }
        }
    }

}
