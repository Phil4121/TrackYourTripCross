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

        public bool SaveItem(T item)
        {
            try
            {
                if (item is IModel)
                {
                    if (!((IModel)item).IsNew)
                    {
                        Con.Delete(item, true);
                    }

                    Con.InsertWithChildren(item, true);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool DeleteItem(T item)
        {
            try
            {
                Con.Delete(item, true);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
