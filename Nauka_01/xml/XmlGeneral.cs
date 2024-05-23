using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_Openness
{


    internal static class XmlGeneral
    {

        


        public static bool IsConvertibleToType(string type, string value) // basic version of checking - data types related to time and data are not sufficient
        {
            switch (type.ToUpperInvariant())
            {
                case "BOOL":
                    return IsBool(value);
                case "BYTE":
                    return IsByte(value);
                case "WORD":
                    return IsWord(value);
                case "INT":
                    return IsInt(value);
                case "REAL":
                    return IsReal(value);
                case "S5TIME":
                    return IsS5Time(value);
                case "TIME":
                    return IsTime(value);
                case "LTIME":
                    return IsLTime(value);
                case "DATE":
                    return IsDate(value);
                case "TIME_OF_DAY":
                    return IsTimeOfDay(value);
                default:
                    throw new ArgumentException($"Unknown type: {type}");
            }
        }


        public static bool IsBool(string value)
        {
            return value.Equals("true", StringComparison.OrdinalIgnoreCase) || 
                value.Equals("false", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsByte(string value)
        {
            if (byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out byte result))
            {
                return result >= byte.MinValue && result <= byte.MaxValue;
            }
            return false;
        }

        public static bool IsWord(string value)
        {
            if (ushort.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out ushort result))
            {
                return result >= ushort.MinValue && result <= ushort.MaxValue;
            }
            return false;
        }

        public static bool IsInt(string value)
        {
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                return result >= int.MinValue && result <= int.MaxValue;
            }
            return false;
        }

        public static bool IsReal(string value)
        {
            if (float.TryParse(value, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out float result))
            {
                return result >= float.MinValue && result <= float.MaxValue;
            }
            return false;
        }

        public static bool IsS5Time(string value)
        {
            return value.StartsWith("S5T#", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsTime(string value)
        {
            return value.StartsWith("T#", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsLTime(string value)
        {
            return value.StartsWith("LT#", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsDate(string value)
        {
            return DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        public static bool IsTimeOfDay(string value)
        {
            return TimeSpan.TryParseExact(value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out _);
        }
    }
}
