using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Globalization;

namespace Assignment2C2P.Common
{
    public static class DevCode
    {
        public static T ToEFItem<T>(this object obj)
        {
            T res = default(T);
            try
            {
                if (obj != null && !string.IsNullOrEmpty(obj.ToString()) && obj is string)
                    res = (T)Convert.ChangeType(obj.ToString().Trim(), typeof(T));
                else if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                    res = (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (Exception ex)
            {
                string message = ex.GetError();
            }
            return res;
        }

        public static string GetError(this Exception ex)
        {
            string res = $"Message : [{ex.Message.Trim()}] || Trace : [{ex.StackTrace.Trim()}]".Trim();
            if (ex.InnerException != null)
            {
                res += $" || InnerMessage : [{ex.InnerException.Message.Trim()}] || InnerTrace : [{ex.InnerException.StackTrace.Trim()}]";
            }
            return res;
        }

        public static string ToJson<T>(this T obj, bool format = false)
        {
            if (obj == null) return string.Empty;
            string res = string.Empty;
            if (format)
                res = JsonConvert.SerializeObject(obj, Formatting.Indented);
            else
                res = JsonConvert.SerializeObject(obj);
            return res;
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            var attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;
        }

        public static bool isDecimal(this object obj)
        {
            decimal d;
            if (decimal.TryParse(obj.ToString(), out d))
            {
                return true;
            }
            return false;
        }

        public static bool isCurrencyCode(this string currencyCode)
        {
            try
            {
                string symbol = string.Empty;
                CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
                foreach (CultureInfo ci in cultures)
                {
                    RegionInfo ri = new RegionInfo(ci.LCID);
                    if (ri.ISOCurrencySymbol == currencyCode)
                    {
                        symbol = ri.CurrencySymbol;
                        //return symbol;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool? TryGetCurrency(string ISOCurrencySymbol)
        {
            string symbol = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture => {
                    try
                    {
                        return new RegionInfo(culture.Name);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
                .Select(ri => ri.CurrencySymbol)
                .FirstOrDefault();
            return symbol != null;
        }

        public static bool isXmlCorrectDateFormat(this string datetime)
        {
            try
            {
                //DateTime dt = DateTime.ParseExact(datetime, "yyyy-MM-ddThh:mm:ss", null);
                DateTime dt = Convert.ToDateTime(datetime);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool isCsvCorrectDateFormat(this string datetime)
        {
            try
            {
                DateTime dt = DateTime.ParseExact(datetime, "dd/MM/yyyy hh:mm:ss", null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool isCorrectDateFormat(this string datetime)
        {
            //yyyy-MM-ddThh:mm:ss
            var dateFormats = new[] { "yyyy-MM-ddThh:mm:ss", "dd/MM/yyyy hh:mm:ss" };
            DateTime scheduleDate;
            bool validDate = DateTime.TryParseExact(
                datetime,
                dateFormats,
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out scheduleDate);
            return validDate;
        }

        public static bool ToBoolFromDB(this int count)
        {
            return count > 0;
        }

        //public static bool isCsvStatus(this string Status)
        //{
        //    try
        //    {
        //        CSVStatus csvStatus = Enum.Parse<CSVStatus>(Status);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public static bool isxmlStatus(this string Status)
        //{
        //    try
        //    {
        //        XmlStatus csvStatus = Enum.Parse<XmlStatus>(Status);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public static bool isStatus(this string Status)
        {
            try
            {
                EnumStatus status = Enum.Parse<EnumStatus>(Status);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
