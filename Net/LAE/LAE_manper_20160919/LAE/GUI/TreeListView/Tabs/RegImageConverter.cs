using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Data;

namespace GUI.TreeListView.Tabs
{
	internal class RegImageConverter : IValueConverter
	{
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
            if (o is ParametroItem)
            {
                return null;
            }
            else {
                AlicuotaItem a = o as AlicuotaItem;
                if(a.Items!=null && a.Items.Count>0)
                    return "../Images/OpenFolderMin.png";
                else
                    return "../Images/ClosedFolderMin.png";
            }
		}


		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
