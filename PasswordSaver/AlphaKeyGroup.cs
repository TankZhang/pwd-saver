using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.Collation;

namespace PasswordSaver
{
    //参考http://edi.wang/post/2016/3/25/windows-10-uwp-fark-pinyin-group?utm_source=tuicool&utm_medium=referral,非常感谢！
    public class AlphaKeyGroup<T> : List<T>, INotifyPropertyChanged
    {
        public delegate string GetKeyDelegate(T item);

        //string _key;
        //public string Key
        //{
        //    get
        //    {
        //        return _key;
        //    }

        //    set
        //    {
        //        _key = value;
        //        RaisedPropertyChanged("Key");
        //    }
        //}
        public string Key { get; private set; }
        //ObservableCollection<T> _internalList;
        //public ObservableCollection<T> InternalList
        //{
        //    get
        //    {
        //        return _internalList;
        //    }

        //    set
        //    {
        //        _internalList = value;
        //        RaisedPropertyChanged("InternalList");

        //    }
        //}
        public List<T> InternalList { get; private set; }

        const string GlobeGroupKey = "??";

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public AlphaKeyGroup(string key)
        {
            this.Key = key;
            InternalList = new List<T>();
        }

        private static List<AlphaKeyGroup<T>> CreateDefaultGroups(CharacterGroupings slg)
        {
            List<AlphaKeyGroup<T>> list = new List<AlphaKeyGroup<T>>();

            foreach (CharacterGrouping cg in slg)
            {
                if (cg.Label == "") continue;
                if (cg.Label == "...")
                {
                    list.Add(new AlphaKeyGroup<T>(GlobeGroupKey));
                }
                else
                {
                    list.Add(new AlphaKeyGroup<T>(cg.Label));
                }
            }

            return list;
        }

        private static List<AlphaKeyGroup<T>> CreateAZGroups()
        {
            char[] alpha = "&#ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var list = alpha.Select(c => new AlphaKeyGroup<T>(c.ToString())).ToList();
            return list;
        }

        public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, Func<T, string> keySelector, bool sort)
        {
            CharacterGroupings slg = new CharacterGroupings();
            //List<AlphaKeyGroup<T>> list = CreateDefaultGroups(slg);
            List<AlphaKeyGroup<T>> list = CreateAZGroups();

            foreach (T item in items)
            {
                int index = 0;
                string[] label = ChineseHelper.GetFirstWord(keySelector(item));
                for (int i = 0; i < label.Length; i++)
                {
                    string lableKey = label[i];
                    index = list.FindIndex(alphaKeyGroup => (alphaKeyGroup.Key.Equals(lableKey, StringComparison.CurrentCulture)));
                    list[index].InternalList.Add(item);
                }
            }

            if (sort)
            {
                foreach (AlphaKeyGroup<T> group in list)
                {
                    group.InternalList.Sort((c0, c1) => { return keySelector(c0).CompareTo(keySelector(c1)); });
                    //group.InternalList = group.InternalList.OrderBy(r => {  return r.WebSite; });
                }
            }

            return list;
        }
    }

}
