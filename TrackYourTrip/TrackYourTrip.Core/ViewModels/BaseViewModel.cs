using Acr.UserDialogs;
using FluentValidation.Results;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Services;

namespace TrackYourTrip.Core.ViewModels
{
    public abstract class BaseViewModel<TParameter> : MvxViewModel
    where TParameter : class

    {
        public BaseViewModel(string title, IMvxLogProvider mvxLogProvider, IMvxNavigationService navigationService)
        {
            this.NavigationService = navigationService;

            this.MvxLogProvider = mvxLogProvider;

            this.Log = mvxLogProvider.GetLogFor(GetType());

            Title = title;

            SaveCommand = new MvxCommand(
                () => SaveTask = MvxNotifyTask.Create(SaveAsync(), onException: ex => LogException(ex))
            );

            DeleteCancelCommand = new MvxCommand(
                () => DeleteTask = MvxNotifyTask.Create(DeleteCancelAsync, onException: ex => LogException(ex))
            );

            AddCommand = new MvxCommand(
                () => AddTask = MvxNotifyTask.Create(AddAsync(), onException: ex => LogException(ex))
            );
        }

        public BaseViewModel(string title, IMvxLogProvider mvxLogProvider, IMvxNavigationService navigationService, IUserDialogs userDialog, ILocalizeService localizeService)
        {
            this.NavigationService = navigationService;

            this.MvxLogProvider = mvxLogProvider;

            this.Log = mvxLogProvider.GetLogFor(GetType());

            this.UserDialog = userDialog;

            this.LocalizeService = localizeService;

            Title = title;

            SaveCommand = new MvxCommand(
                () => SaveTask = MvxNotifyTask.Create(SaveAsync(), onException: ex => LogException(ex))
            );

            DeleteCancelCommand = new MvxCommand(
                () => DeleteTask = MvxNotifyTask.Create(DeleteCancelAsync, onException: ex => LogException(ex))
            );

            AddCommand = new MvxCommand(
                () => AddTask = MvxNotifyTask.Create(AddAsync(), onException: ex => LogException(ex))
            );
        }

        #region Properties

        public abstract IDataServiceFactory<TParameter> DataStore { get; set; }

        public IMvxNavigationService NavigationService { get; private set; }

        public IMvxLogProvider MvxLogProvider { get; private set; }

        public IMvxLog Log { get; private set; }

        public IUserDialogs UserDialog { get; private set; }

        public ILocalizeService LocalizeService { get; private set; }

        public string Title { get; set; }

        bool _isBusy = false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public abstract bool IsNew { get; }

        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            private set { SetProperty(ref _isValid, value); }
        }

        private string _errors;
        public string Errors
        {
            get { return _errors; }
            private set { SetProperty(ref _errors, value); }
        }

        private ValidationResult _validationResult;
        public ValidationResult ValidationResult
        {
            get { return _validationResult; }
            set
            {
                SetProperty(ref _validationResult, value);
                IsValid = _validationResult.IsValid;
                Errors = _validationResult.ToString();
            }
        }

        #endregion

        #region Tasks

        public MvxNotifyTask AddTask { get; private set; }

        public MvxNotifyTask SaveTask { get; private set; }

        public MvxNotifyTask DeleteTask { get; private set; }

        #endregion

        #region Commands

        public IMvxCommand AddCommand { get; private set; }

        public IMvxCommand SaveCommand { get; private set; }

        public IMvxCommand DeleteCancelCommand { get; private set; }

        #endregion

        #region Methods
        public abstract void Validate();

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            IsBusy = false;
        }


        public async virtual Task SaveAsync()
        {
            Validate();
        }

        public string SaveCommandTitle
        {
            get => Resources.AppResources.SaveCommandTitle;
        }


        public async virtual Task AddAsync()
        {

        }

        public string AddCommandTitle
        {
            get => Resources.AppResources.AddCommandTitle;
        }


        public async virtual Task DeleteCancelAsync()
        {

        }

        public string DeleteCancelCommandTitle
        {
            get
            {
                if (IsNew)
                    return Resources.AppResources.CancelCommandTitle;

                return Resources.AppResources.DeleteCommandTitle;
            }
        }


        public void LogException(Exception ex)
        {
            Log.ErrorException(ex.Message, ex);
        }

        #endregion
    }

    public abstract class BaseViewModel<TParameter, TResult> : BaseViewModel<TParameter>, IMvxViewModel<TParameter, TResult>
        where TParameter : class
        where TResult : class

    {
        public BaseViewModel(string title, IMvxLogProvider mvxLogProvider, IMvxNavigationService navigationService)
            : base(title, mvxLogProvider, navigationService)
        {

        }

        public BaseViewModel(string title, IMvxLogProvider mvxLogProvider, IMvxNavigationService navigationService, IUserDialogs userDialog, ILocalizeService localizeService)
            : base(title, mvxLogProvider, navigationService, userDialog, localizeService)
        {

        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public virtual void Prepare(TParameter parameter)
        {

        }
    }
}
