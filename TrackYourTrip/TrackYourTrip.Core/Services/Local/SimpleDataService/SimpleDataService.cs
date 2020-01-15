using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using TrackYourTrip.Core.Interfaces;

namespace TrackYourTrip.Core.Services
{
    public class SimpleDataService<T> : IDataServiceFactory<T> where T : new()
    {
        private string TableName { get; set; }

        public SimpleDataService(SQLiteConnection con, string tableName)
        {
            this.Con = con;
            this.TableName = tableName;
        }

        private SQLiteConnection _Con;
        public SQLiteConnection Con
        {
            get
            {
                return _Con;
            }
            
            private set
            {
                _Con = value;
            }
        }

        public async Task<T> GetItemAsync(Guid id)
        {
            return await Task.FromResult(Con.GetWithChildren<T>(id));
        }

        public async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(Con.GetAllWithChildren<T>());
        }

        public async Task<bool> SaveItemAsync(T item)
        {
            if (item is IModel)
            {
                if(!((IModel)item).IsNew)
                    return await Task.FromResult(Con.Update(item)) > 0;
            }

            return await Task.FromResult(Con.Insert(item)) > 0;
        }

        public async Task<bool> DeleteItemAsync(T item)
        {
            return await Task.FromResult(Con.Delete(item)) > 0;
        }
    }
}
