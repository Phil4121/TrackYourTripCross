using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services.BackgroundQueue.Messages;
using TrackYourTrip.Core.Services.Weather;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Services.BackgroundQueue
{
    public static class BackgroundWorkerService
    {
        static bool _isRunning;
        public static bool IsRunning { get; }

        public static async Task RunBackgroundWorkerService(CancellationToken token)
        {
            await RunBackgroundWorkerService(DataServiceFactory.Connection, token);
        }

        public static async Task RunBackgroundWorkerService(SQLiteConnection connection, CancellationToken token)
        {
            if (IsRunning)
                return;

            await Task.Run(async () =>
            {
                _isRunning = true;
                

                var cnt = await BackgroundQueueService.GetQueueElementCount(connection);
                BackgroundTaskModel element = null;

                while (cnt > 0)
                {
                    token.ThrowIfCancellationRequested();

                    element = await BackgroundQueueService.PopFromBackgroundQueue();

                    if(!element.ProcessedSuccessfully)
                        await ProzessElement(element, token);

                    if (element.ProcessedSuccessfully)
                        await BackgroundQueueService.RemoveElementFromQueue(element);

                    cnt = cnt - 1;
                }

                _isRunning = false;

            }, token);
        }

        static Task<BackgroundTaskModel> ProzessElement(BackgroundTaskModel model, CancellationToken token)
        {
            return Task.Run(() =>
            {
                switch (model.ID_TaskType)
                {
                    case (int) EnumHelper.TaskTypeEnum.WheaterTask:
                        ProzessWheaterRequest(ref model);
                        break;

                    default:
                        throw new Exception("TaskType not valid!");
                }

                if (model.ProcessedSuccessfully)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send(new ElementFinishedMessage()
                        {
                            BackgroundTask = model
                        }, MessageHelper.ELEMENT_FINISHED_MESSAGE);
                    });
                }

                return model;
            }, token);
        }

        static void ProzessWheaterRequest(ref BackgroundTaskModel model)
        {
            try
            {
                IWeatherService ws = WeatherServiceFactory.GetWeatherServiceFactory();

                var response = ws.GetWeatherData(
                    new JSONHelper<WeatherTaskRequestModel>().Deserialize(
                        model.TaskData)
                    );

                if(response != null)
                {
                    model.TaskResponse = new JSONHelper<WeatherTaskResponseModel>().Serialize(response.Result);
                    model.ProcessedSuccessfully = true;
                    return;
                }

                model.ProcessedSuccessfully = false;

            }catch(Exception ex)
            {
                throw;
            }
        }
    }
}
