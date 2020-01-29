using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IDataServiceFactory<T>
    {
        Task<T> SaveItemAsync(T item, CancellationToken cancellationToken = default);
        Task<bool> DeleteItemAsync(T item);
        Task<T> GetItemAsync(Guid id);
        Task<IEnumerable<T>> GetItemsAsync();
    }
}
