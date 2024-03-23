using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace KaibaSystem_Back_End.Extensions.Helpers
{
    public static class Extensions
    {
        public static T To<T>(this IConvertible value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        public static DateTime ToDateTime(this string value)
        {
            var data = value.Split("-");
            return new DateTime(
                                int.Parse(data[0]),
                                int.Parse(data[1]),
                                int.Parse(data[2]));
        }

        public static decimal ToDecimal(this string value)
        {
            decimal number;

            decimal.TryParse(value, out number);

            return number;
        }

        public static bool IsNull(this object source)
        {
            return source == null;
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return !source.IsNotNullOrEmpty();
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string input)
        {
            return string.IsNullOrEmpty(input) || input.Trim() == string.Empty;
        }

        public static IEnumerable<T> AlphaLengthWise<T, L>(this IEnumerable<T> names, Func<T, L> lengthProvider)
        {
            return names.OrderBy(a => lengthProvider(a)).ThenBy(a => a);
        }

        public static int Compare<T>(this T value, T value2) where T : IComparable
        {
            return value.CompareTo(value2);
        }

        public static bool AnyOfType<T>(this IEnumerable source)
        {
            foreach (var obj in source)
            {
                if (obj is T)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Like(this string value, string search)
        {
            return value.Contains(search) || value.StartsWith(search) || value.EndsWith(search);
        }

        /// <summary>
        /// Formats the string according to the specified mask
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
        /// <returns>The formatted string</returns>
        public static string FormatWithMask(this string input, string mask)
        {
            if (input.IsNullOrEmpty()) return input;
            var output = string.Empty;
            var index = 0;
            foreach (var m in mask)
            {
                if (m == '#')
                {
                    if (index < input.Length)
                    {
                        output += input[index];
                        index++;
                    }
                }
                else
                    output += m;
            }
            return output;
        }

        public static string StripHtml(this string input)
        {
            var tagsExpression = new Regex(@"</?.+?>");
            return tagsExpression.Replace(input, " ");
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = rec.GetType().GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target)
        {
            Random r = new Random();

            return target.OrderBy(x => r.Next());
        }

        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public static bool IsWeekday(this DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Sunday:
                case DayOfWeek.Saturday:
                    return false;

                default:
                    return true;
            }
        }

        public static bool IsWeekend(this DayOfWeek dow)
        {
            return !dow.IsWeekday();
        }

        public static DateTime AddWorkdays(this DateTime startDate, int days)
        {
            // start from a weekday
            while (startDate.DayOfWeek.IsWeekend())
            {
                startDate = startDate.AddDays(1.0);
            }
            for (int i = 0; i < days; ++i)
            {
                startDate = startDate.AddDays(1.0);

                while (startDate.DayOfWeek.IsWeekend())
                    startDate = startDate.AddDays(1.0);
            }
            return startDate;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static bool IsPrime(this int number)
        {
            if (number % 2 == 0)
            {
                return number == 2;
            }
            int sqrt = (int)Math.Sqrt(number);
            for (int t = 3; t <= sqrt; t = t + 2)
            {
                if (number % t == 0)
                {
                    return false;
                }
            }
            return number != 1;
        }

        public static string SerializeToXml(this object obj)
        {
            XDocument doc = new XDocument();
            using (XmlWriter xmlWriter = doc.CreateWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(xmlWriter, obj);
                xmlWriter.Close();
            }
            return doc.ToString();
        }

        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static string ToCSV<T>(this IEnumerable<T> instance, char separator)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(value => csv.AppendFormat("{0}{1}", value, separator));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static string ToCSV<T>(this IEnumerable<T> instance)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(v => csv.AppendFormat("{0},", v));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static IEnumerable<T> RemoveDuplicates<T>(this ICollection<T> list, Func<T, int> Predicate)
        {
            var dict = new Dictionary<int, T>();

            foreach (var item in list)
            {
                if (!dict.ContainsKey(Predicate(item)))
                {
                    dict.Add(Predicate(item), item);
                }
            }

            return dict.Values.AsEnumerable();
        }

        public static string GetMethodName(this MethodBase method)
        {
            string _methodName = method.DeclaringType.FullName;

            if (_methodName.Contains(">") || _methodName.Contains("<"))
            {
                _methodName = _methodName.Split('<', '>')[1];
            }
            else
            {
                _methodName = method.Name;
            }

            return _methodName;
        }

        public static string GetClassName(this MethodBase method)
        {
            string className = method.DeclaringType.FullName;

            if (className.Contains(">") || className.Contains("<"))
            {
                className = className.Split('+')[0];
            }
            return className;
        }

        public static string GetUniqueFileName(this string OriginalName)
        {
            string Extension = Path.GetExtension(OriginalName).ToLower();
            Guid guid = Guid.NewGuid();
            return $"{DateTime.Now.ToString("ddMMyyyy")}-{guid.ToString().Replace("-", "").Substring(0, 5)}{Extension}";
        }

        public static string RemoveNonNumericDigits(this string str)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }

        public static string GetErrorMessage(this Exception ex)
        {
            string errors = "";
            Exception exception = ex;
            do
            {
                errors += $"{exception.Message}\n";
                exception = exception.InnerException;
            } while (exception != null);

            return errors;
        }

        public static List<List<T>> SplitList<T>(this IList<T> source, int length)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / length)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}