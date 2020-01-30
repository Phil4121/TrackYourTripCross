using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Database.UnitTests
{
    public static class Helper
    {
        public static List<string> GetTableNameConstants()
        {
            List<FieldInfo> allTableConstants = GetConstants(typeof(TrackYourTrip.Core.Helpers.TableConsts));

            List<string> tableNames = new List<string>();

            foreach (FieldInfo fi in allTableConstants)
            {
                if (fi.Name.EndsWith("_TABLE") &&
                    !fi.Name.StartsWith("FK_"))
                {

                    object value = fi.GetValue(null);

                    if (!string.IsNullOrEmpty(value.ToString()))
                    {
                        tableNames.Add(value.ToString());
                    }
                }
            }

            return tableNames;
        }
        private static List<FieldInfo> GetConstants(Type type)
        {
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
                 BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
        }
    }
}
