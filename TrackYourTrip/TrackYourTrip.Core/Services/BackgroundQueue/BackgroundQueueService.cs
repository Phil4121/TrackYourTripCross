using Database;
using MvvmCross.Binding.Extensions;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.Services.BackgroundQueue
{
    public class BackgroundQueueService
    {
        private SQLiteConnection Connection { get; set; }

        private SimpleDataService<BackgroundTaskModel> Service { get; set; }

        public async Task<bool> PushWheaterRequestToBackgroundQueue(Guid SpotId, double Lat, double Lng)
        {
            return await PushWheaterRequestToBackgroundQueue(DataServiceFactory.Connection, SpotId, Lat, Lng);
        }

        public async Task<bool> PushWheaterRequestToBackgroundQueue(SQLiteConnection connection, Guid SpotId, double Lat, double Lng)
        {
            this.Connection = connection;

            try
            {
                var queueElement = new BackgroundTaskModel(true)
                {
                    ID_TaskType = (int)EnumHelper.TaskTypeEnum.WheaterTask,
                    ID_ElementReference = SpotId,
                    CreationDateTime = DateTime.Now,
                    TaskData = new JSONHelper<WheaterTaskModel>().Serialize(
                        new WheaterTaskModel()
                        {
                            RequestDateTime = DateTime.Now,
                            RequestLat = Lat,
                            RequestLng = Lng
                        })
                };

                return await PushToBackgroundQueue(queueElement) != null;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<BackgroundTaskModel> PopWheaterRequestFromBackgroundQueue(SQLiteConnection connection, bool getLatest = true)
        {
            this.Connection = connection;

            return await PopFromBackgroundQueue((int)EnumHelper.TaskTypeEnum.WheaterTask, getLatest);
        }

        public async Task<bool> RemoveElementFromQueue(SQLiteConnection connection, BackgroundTaskModel model)
        {
            try
            {
                Connection = connection;

                Service = new SimpleDataService<BackgroundTaskModel>(Connection, TableConsts.BACKGROUND_TASK_TABLE);
                return await Service.DeleteItemAsync(model);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetQueueElementCount(SQLiteConnection connection)
        {
            this.Connection = connection;

            Service = new SimpleDataService<BackgroundTaskModel>(Connection, TableConsts.BACKGROUND_TASK_TABLE);
            var queue = await Service.GetItemsAsync();

            return queue.Count();
        }

        public async Task<bool> EmptyQueue(SQLiteConnection connection)
        {
            this.Connection = connection;

            Service = new SimpleDataService<BackgroundTaskModel>(Connection, TableConsts.BACKGROUND_TASK_TABLE);
            return await Service.DeleteItemsAsync();
        }

        public Task<BackgroundTaskModel> PopFromBackgroundQueue(bool getLatest = true)
        {
            return PopFromBackgroundQueue(-1, getLatest);
        }

        async Task<BackgroundTaskModel> PushToBackgroundQueue(BackgroundTaskModel model)
        {
            try
            {
                Service = new SimpleDataService<BackgroundTaskModel>(Connection, TableConsts.BACKGROUND_TASK_TABLE);
                return await Service.SaveItemAsync(model);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task<BackgroundTaskModel> PopFromBackgroundQueue(int TaskTypeId, bool getLatest = true)
        {
            Service = new SimpleDataService<BackgroundTaskModel>(Connection, TableConsts.BACKGROUND_TASK_TABLE);
            var backgroundQueue = await Service.GetItemsAsync();

            var elements = backgroundQueue;

            BackgroundTaskModel popedElement = null;

            if(TaskTypeId != -1)
                elements = backgroundQueue.Where(t => t.ID_TaskType == TaskTypeId);

            if (getLatest)
                popedElement = elements
                    .OrderByDescending(x => x.CreationDateTime)
                    .FirstOrDefault();

            popedElement = elements
                .OrderBy(x => x.CreationDateTime)
                .FirstOrDefault();

            if (popedElement != null)
                await RemoveElementFromQueue(Connection, popedElement);

            return popedElement;
        }
    }
}
