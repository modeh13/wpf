using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfDrawingTool.Converts
{
   public class StatusConverter : IValueConverter
   {
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         string input = value as string;
         switch (input)
         {
            case "INFO":
               return DependencyProperty.UnsetValue;
            case "WARN":
               return Brushes.Yellow;               
            case "ERROR":
               return Brushes.Red;               
            default:
               return DependencyProperty.UnsetValue;
         }
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotSupportedException();
      }
   }
}