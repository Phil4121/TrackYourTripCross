using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrackYourTrip.Core.Helpers
{
    public class JSONHelper<T>
    {
        public string Serialize(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public T Deserialize(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);

            }catch (Exception ex)
            {
                throw;
            }
        }
    }
}
