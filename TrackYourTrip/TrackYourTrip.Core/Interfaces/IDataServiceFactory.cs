using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IDataServiceFactory<T>
    {
        T SaveItem(T item);
        bool DeleteItem(T item);
        Task<T> GetItemAsync(Guid id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
