using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Newtonsoft.Json;

namespace OOSD2_CA1___Thiago_Braz
{

        // Converts enum values to boolean for WPF data binding.
        class EnumBooleanConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value == null || parameter == null)
                    return false;

                if (!Enum.IsDefined(value.GetType(), value))
                    return false;

                object enumValue = Enum.Parse(value.GetType(), parameter.ToString());
                return value.Equals(enumValue);
            }

            // Converts a boolean back to an enum value
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if ((bool)value)
                    return Enum.Parse(targetType, parameter.ToString());

                return Binding.DoNothing;
            }
        }
    }
