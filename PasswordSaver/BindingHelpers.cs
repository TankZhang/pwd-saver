using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PasswordSaver
{
    //如果string不为空，则显示，否则不显示
    public class StringVisibleBindingHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return String.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    //如果为true，返回Visibility.Collapsed，否则Visibility.Visible
    public class CollapseBindingHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Visibility)value == Visibility.Collapsed)
                return true;
            else
                return false;
        }
    }

    //如果为true，返回Visibility.Visible，否则返回Visibility.Collapsed
    public class VisibleBindingHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((Visibility)value == Visibility.Collapsed)
                return false;
            else
                return true;
        }
    }

   public class FontSizeBindingHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch ((string)parameter)
            {
                case "CalFontSize":
                    return ((((double)value) * 2) / 3);
                default:
                    return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
