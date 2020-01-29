using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading;
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

        public SQLiteConnection Con { get; private set; }

        public Task<T> GetItemAsync(Guid id)
        {
            Task<T> getItemTask = default;
            getItemTask = Task.Run(() =>
            {
                return Con.GetWithChildren<T>(id, true);
            });

            return getItemTask;
        }

        public Task<IEnumerable<T>> GetItemsAsync()
        {
            Task<IEnumerable<T>> getItemsTask = default;
            getItemsTask = Task.Run(() =>
            {
                return Con.GetAllWithChildren<T>().AsEnumerable();
            });

            return getItemsTask;
        }

        public Task<T> SaveItemAsync(T item, CancellationToken cancellationToken = default)
        {
            Task<T> workerTask = default;
            workerTask = Task.Run(() =>
            {
                try
                {
                    if (item is IModel)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (!((IModel)item).IsNew)
                            Con.Delete(item, true);

                        Con.InsertWithChildren(item, true);

                        return Con.GetWithChildren<T>(((IModel)item).Id);
                    }

                    throw new Exception("Item does not implement IModel");
                }
                catch (Exception ex)
                {
                    throw;
                }
            },cancellationToken: cancellationToken);

            return workerTask;




            
        }

        public Task<bool> DeleteItemAsync(T item)
        {
            Task<bool> deleteItemTask = default;
            deleteItemTask = Task.Run(() =>
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
            });

            return deleteItemTask;
        }
    }
}
