﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.Interfaces
{
    public interface IDataServiceFactory<T> 
    {
        Task<T> SaveItemAsync(T item, CancellationToken cancellationToken = default);
        Task<bool> DeleteItemAsync(T item);

        Task<bool> DeleteItemsAsync();

        Task<T> GetItemAsync(Guid id);
        Task<IEnumerable<T>> GetItemsAsync();
    }
}
