using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.Forms;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TrackYourTrip.Core.Services.BackgroundQueue;
using TrackYourTrip.Core.Services.BackgroundQueue.Messages;

namespace TrackYourTrip.Droid.Services.BackgroundService
{
    [Service]
    public class BackgroundTaskService : Service
    {
		CancellationTokenSource _cts;

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			_cts = new CancellationTokenSource();

			Task.Run(() => {
				try
				{
					BackgroundWorkerService.RunBackgroundWorkerService(_cts.Token).Wait();
				}
				catch (System.OperationCanceledException ex)
				{
					throw;
				}
				finally
				{
					if (_cts.IsCancellationRequested)
					{
						var message = new CancelledMessage();
						Device.BeginInvokeOnMainThread(
							() => MessagingCenter.Send(message, "CancelledMessage")
						);
					}
				}

			}, _cts.Token);

			return StartCommandResult.Sticky;
		}

		public override void OnDestroy()
		{
			if (_cts != null)
			{
				_cts.Token.ThrowIfCancellationRequested();

				_cts.Cancel();
			}
			base.OnDestroy();
		}
	}
}