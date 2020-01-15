using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Database.UnitTests
{
    public static class Helper
    {
        public static List<string> GetTableNameConstants()
        {
            var allTableConstants = GetConstants(typeof(TrackYourTrip.Core.Helpers.TableConsts));

            var tableNames = new List<string>();

            foreach(FieldInfo fi in allTableConstants)
            {
                if (fi.Name.EndsWith("_TABLE") &&
                    !fi.Name.StartsWith("FK_")) {
                    
                    var value = fi.GetValue(null);

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
