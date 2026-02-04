using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web;

namespace Application.Extensions
{
    public static class AppExtensions
    {
        public static string ToQueryString(this object obj)
        {
            if (obj == null) return string.Empty;

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value == null) continue;

                query[prop.Name] = value.ToString();
            }

            return query.ToString()!;
        }
    }
}
