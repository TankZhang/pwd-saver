using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.Collation;

namespace UWPTest
{
    /// <summary>
    /// 用于实现按首字母分组
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class AlphaKeyGroup<T> : List<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// 用来获取Key的委托
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public delegate string GetKeyDelegate(T item);

        string _key;
        public string Key
        {
            get
            {
                return _key;
            }

            set
            {
                _key = value;
                RaisedPropertyChanged("Key");
            }
        }
        //public string Key { get; private set; }

        ObservableCollection<T> _internalList;
        public ObservableCollection<T> InternalList
        {
            get
            {
                return _internalList;
            }

            set
            {
                _internalList = value;
                RaisedPropertyChanged("InternalList");

            }
        }


        const string GlobeGroupKey = "??";

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        //public List<T> InternalList { get; private set; }


        public AlphaKeyGroup(string key)
        {
            this.Key = key;
            InternalList =new ObservableCollection<T>();
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
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
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
                {
                    string label = ChineseHelper.GetFirstWord(keySelector(item));
                    //string label = slg.Lookup(keySelector(item));
                    index = list.FindIndex(alphaKeyGroup => (alphaKeyGroup.Key.Equals(label, StringComparison.CurrentCulture)));
                }

                if (index >= 0 && index < list.Count)
                {
                    list[index].InternalList.Add(item);
                }
                else
                {
                    //临时解决方案，中文加入？？中
                    list[list.Count - 1].InternalList.Add(item);
                }
            }

            if (sort)
            {
                foreach (AlphaKeyGroup<T> group in list)
                {
                    //group.InternalList.Sort((c0, c1) => { return keySelector(c0).CompareTo(keySelector(c1)); });
                    //group.InternalList = group.InternalList.OrderBy(r => {  return r.WebSite; });
                }
            }

            return list;
        }
    }

}
