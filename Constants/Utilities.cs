using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Constants.Interface;
using Constants.Security;
using Newtonsoft.Json;

namespace Constants
{
    public static class Utilities
    {
        #region Helper Functions

        public static T Cast<T>(this object myobj)
        {
            var objectType = myobj.GetType();
            var target = typeof(T);
            var x = Activator.CreateInstance(target, false);
            var z = from source in objectType.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var d = from source in target.GetMembers().ToList()
                    where source.MemberType == MemberTypes.Property
                    select source;
            var members = d.Where(memberInfo => d.Select(c => c.Name)
                .ToList().Contains(memberInfo.Name)).ToList();

            foreach (var memberInfo in members)
            {
                var propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                var value = myobj.GetType().GetProperty(memberInfo.Name)?.GetValue(myobj, null);

                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(x, value, null);
                }
            }
            return (T)x;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static List<T> GetAllPublicConstantValues<T>(this Type type)
        {
            return type
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                .Select(x => (T)x.GetRawConstantValue())
                .ToList();
        }

        public static string ConcatStringWithThreeDot(this string title, int maxLength = 40)
        {
            if (title.Length > maxLength)
            {
                title = string.Concat(title.Substring(0, maxLength), "...");
            }
            return title;
        }

        public static string TitleShortener(this string title)
        {
            int maxLength = 50;
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            title = title.Normalize(NormalizationForm.FormD);

            string temp = regex.Replace(title, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            //replace special characters
            temp = Regex.Replace(temp, @"[^\w\d]", "-");
            temp = temp.Replace("---", "-").Replace("--", "-");


            //char[] separators = { '~','`','#', '%', '&', '{', '}','[', ']', '\\', '<', '>', '*', '?', '/', ' ', '$', '!', '\'', '"', ':', '@','.',',' };
            //string[] str = temp.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //temp = String.Join("-", str);

            if (temp.Length > maxLength)
            {
                temp = temp.Substring(0, maxLength);
            }

            return temp;
        }

        // Random code for verify, password..v..v.. - Create 4/8/2017 - Tri
        public static string RandomString(int length = 6, string type = "number")
        {
            // 3 type: number, character, combine
            var random = new Random();
            string chars = "";

            if (type == "number")
            {
                chars = "0123456789";
            }
            else if (type == "character")
            {
                chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }
            else if (type == "combine")
            {
                chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            }

            var ReturnValue = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return ReturnValue;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// 3 cách remove thẻ HTML trong một chuỗi
        /// Nguồn: https://www.dotnetperls.com/remove-html-tags
        /// </summary>
        public static class RemoveHTMLTagFromString
        {
            /// <summary>
            /// Remove HTML from string with Regex.
            /// </summary>
            public static string StripTagsRegex(string source)
            {
                return Regex.Replace(source, "<.*?>", string.Empty);
            }

            /// <summary>
            /// Compiled regular expression for performance.
            /// </summary>
            static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

            /// <summary>
            /// Remove HTML from string with compiled Regex.
            /// </summary>
            public static string StripTagsRegexCompiled(string source)
            {
                return _htmlRegex.Replace(source, string.Empty);
            }

            /// <summary>
            /// Remove HTML tags from string using char array.
            /// </summary>
            public static string StripTagsCharArray(string source)
            {
                char[] array = new char[source.Length];
                int arrayIndex = 0;
                bool inside = false;

                for (int i = 0; i < source.Length; i++)
                {
                    char let = source[i];
                    if (let == '<')
                    {
                        inside = true;
                        continue;
                    }
                    if (let == '>')
                    {
                        inside = false;
                        continue;
                    }
                    if (!inside)
                    {
                        array[arrayIndex] = let;
                        arrayIndex++;
                    }
                }
                return new string(array, 0, arrayIndex);
            }
        }
        
        #endregion

        #region Custom Utilities
        public static string CreateLoginToken(ILoginAccount account)
        {
            var loginStr = JsonConvert.SerializeObject(account);

            var encryptor = new Encryptor.AESencrypt();
            return encryptor.EncryptString_Aes(loginStr);
        }

        public static ILoginAccount ParseLoginToken(string token)
        {
            var encryptor = new Encryptor.AESdecrypt();
            var loginStr = encryptor.DecryptString_Aes(token);

            return JsonConvert.DeserializeObject<ILoginAccount>(loginStr);
        }
        #endregion

    }
}
