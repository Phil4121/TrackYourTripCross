using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IDataServiceFactory<T>
    {
        Task<bool> SaveItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<T> GetItemAsync(Guid id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
