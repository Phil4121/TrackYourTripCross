using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services.BackgroundQueue.Messages;
using Xamarin.Forms;

namespace TrackYourTrip.Core.Services.BackgroundQueue
{
    public static class BackgroundWorkerService
    {
        static BackgroundQueueService _service;

        static BackgroundQueueService Service
        {
            get
            {
                if (_service == null)
                    _service = new BackgroundQueueService();

                return _service;
            }
        }

        static bool _isRunning;
        public static bool IsRunning { get; }

        public static async Task RunBackgroundWorkerService(CancellationToken token)
        {
            await RunBackgroundWorkerService(DataServiceFactory.Connection, token);
        }

        public static async Task RunBackgroundWorkerService(SQLiteConnection connection, CancellationToken token)
        {
            await Task.Run(async () =>
            {
                _isRunning = true;

                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagingCenter.Send(new StartBackgroundWorkingServiceMessage(), "StartBackgroundWorkingServiceMessage");
                });

                var cnt = await Service.GetQueueElementCount(connection);

                while (cnt > 0)
                {
                    token.ThrowIfCancellationRequested();

                    var prozessedElement = await ProzessElement(await Service.PopFromBackgroundQueue(), token);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MessagingCenter.Send(new ElementFinishedMessage() { 
                            BackgroundTask = prozessedElement 
                        } , "ElementFinishedMessage");
                    });

                    cnt = await Service.GetQueueElementCount(connection);
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    MessagingCenter.Send(new StopBackgroundWorkingServiceMessage(), "StopBackgroundWorkingServiceMessage");
                });

                _isRunning = false;

            }, token);
        }

        static Task<BackgroundTaskModel> ProzessElement(BackgroundTaskModel model, CancellationToken token)
        {
            return Task.Run(async () =>
            {
                return model;
            }, token);
        }
    }
}
