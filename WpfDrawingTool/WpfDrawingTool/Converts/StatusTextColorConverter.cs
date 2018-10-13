using System;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfDrawingTool.Converts
{
   public class StatusTextColorConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         string input = value as string;
         switch (input)
         {
            case "ERROR":
               return Brushes.White;
            default:
               return Brushes.Black;
         }
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotSupportedException();
      }
   }
}