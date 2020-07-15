using Database;
using MvvmCross.Binding.Extensions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Models;

namespace TrackYourTrip.Core.Services.BackgroundQueue
{
    public static class BackgroundQueueService
    {
        private static SQLiteConnection Connection { get; set; }

        private static SimpleDataService<BackgroundTaskModel> Service { get; set; }

        public static async Task<Guid> PushWheaterRequestToBackgroundQueue(Guid RefId, double Lat, double Lng)
        {
            return await PushWheaterRequestToBackgroundQueue(DataServiceFactory.Connection, RefId, Lat, Lng);
        }

        public static async Task<Guid> PushWheaterRequestToBackgroundQueue(SQLiteConnection connection, Guid RefId, double Lat, double Lng)
        {
            Connection = connection;

            try
            {
                var queueElement = new BackgroundTaskModel(true)
                {
                    ID_TaskType = (int)EnumHelper.TaskTypeEnum.WheaterTask,
                    ID_ElementReference = RefId,
                    CreationDateTime = DateTime.Now,
                    TaskData = new JSONHelper<WeatherTaskRequestModel>().Serialize(
                        new WeatherTaskRequestModel()
                        {
                            CultureInfo = CultureInfo.CurrentCulture,
                            RequestDateTime = DateTime.Now,
                            Lat = Lat,
                            Lng = Lng
                        })
                };

                if (await PushToBackgroundQueue(queueElement) != null)
                    return queueElement.Id;
                else
                    return Guid.Empty;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<BackgroundTaskModel> PopWheaterRequestFromBackgroundQueue(SQLiteConnection connection, bool getLatest = true)
        {
            Connection = connection;

            return await PopFromBackgroundQueue((int)EnumHelper.TaskTypeEnum.WheaterTask, getLatest);
        }

        public static async Task<bool> RemoveElementFromQueue(BackgroundTaskModel model)
        {
            return await RemoveElementFromQueue(Connection, model);
        }

            public static async Task<bool> RemoveElementFromQueue(SQLiteConnection connection, BackgroundTaskModel model)
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

        public static async Task<bool> RemoveElementFromQueue(Guid id)
        {
            var model = await GetElementById(id);

            if (model != null)
                return await RemoveElementFromQueue(model);

            return true;
        }

        public static async Task<int> GetQueueElementCount(SQLiteConnection connection)
        {
            Connection = connection;

            Service = new SimpleDataService<BackgroundTaskModel>(Connection, TableConsts.BACKGROUND_TASK_TABLE);
            var queue = await Service.GetItemsAsync();

            return queue.Count();
        }

        public static async Task<bool> EmptyQueue(SQLiteConnection connection)
        {
            Connection = connection;

            Service = new SimpleDataService<BackgroundTaskModel>(Connection, TableConsts.BACKGROUND_TASK_TABLE);
            return await Service.DeleteItemsAsync();
        }

        public static Task<BackgroundTaskModel> PopFromBackgroundQueue(bool getLatest = true)
        {
            return PopFromBackgroundQueue(-1, getLatest);
        }

        async static Task<BackgroundTaskModel> PushToBackgroundQueue(BackgroundTaskModel model)
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

        async static Task<BackgroundTaskModel> PopFromBackgroundQueue(int TaskTypeId, bool getLatest = true)
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

        async static Task<BackgroundTaskModel> GetElementById(Guid id)
        {
            Service = new SimpleDataService<BackgroundTaskModel>(Connection, TableConsts.BACKGROUND_TASK_TABLE);
            return await Service.GetItemAsync(id);
        }
    }
}
