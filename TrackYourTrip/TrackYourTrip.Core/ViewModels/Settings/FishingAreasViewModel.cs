﻿using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Core.Services;
using TrackYourTrip.Core.ViewModelResults;
using TrackYourTrip.Core.ViewModels.Settings;

[assembly: MvxNavigation(typeof(FishingAreasViewModel), @"FishingAreasPage")]
namespace TrackYourTrip.Core.ViewModels.Settings
{
    public class FishingAreasViewModel : BaseViewModel<FishingAreaModel>
    {
        public FishingAreasViewModel(IMvxNavigationService navigationService, IMvxLogProvider mvxLogProvider)
            : base(Resources.AppResources.FishingAreasPageTitle, mvxLogProvider, navigationService)
        {
            FishingAreaSelectedCommand = new MvxAsyncCommand<FishingAreaModel>(NavigateToFishingArea);
        }

        #region Properties

        private IDataServiceFactory<FishingAreaModel> _dataStore;
        public override IDataServiceFactory<FishingAreaModel> DataStore
        {
            get
            {
                if (_dataStore == null)
                    _dataStore = DataServiceFactory.GetFishingAreaFactory();

                return _dataStore;
            }
            set { _dataStore = value; }
        }

        
        private MvxObservableCollection<FishingAreaModel> _fishingAreas;
        public MvxObservableCollection<FishingAreaModel> FishingAreas
        {
            get => _fishingAreas;
            private set => SetProperty(ref _fishingAreas, value);
        }

        private FishingAreaModel _selectedFishingArea;
        public FishingAreaModel SelectedFishingArea
        {
            get => _selectedFishingArea;
            private set
            {
                SetProperty(ref _selectedFishingArea, value);
                FishingAreaSelectedCommand.ExecuteAsync(value);
            }
        }

        public override bool IsNew => false;

        #endregion

        #region Commands

        public IMvxAsyncCommand<FishingAreaModel> FishingAreaSelectedCommand { get; private set; }

        #endregion

        #region Tasks

        public MvxNotifyTask LoadAreasTask { get; private set; }

        #endregion

        #region Methodes

        public override Task Initialize()
        {
            LoadAreasTask = MvxNotifyTask.Create(LoadAreasAsync(), onException: ex => LogException(ex));
            return base.Initialize();
        }

        public override void Validate()
        {
            throw new NotImplementedException();
        }

        public override void Add()
        {
            base.Add();

            MvxNotifyTask.Create(NavigateToFishingArea(new FishingAreaModel(true)), onException: ex => LogException(ex));
        }

        async Task NavigateToFishingArea(FishingAreaModel fishingArea)
        {
            if (fishingArea == null)
                return;

            IsBusy = true;
            
            var result = await NavigationService.Navigate<FishingAreaViewModel, FishingAreaModel, OperationResult<FishingAreaModel>>(fishingArea);
            IsBusy = false;

            if (result != null)
            {
                if (result.IsCanceld)
                    return;

                LoadAreasTask = MvxNotifyTask.Create(LoadAreasAsync, onException: ex => LogException(ex));
            }
        }

        async Task LoadAreasAsync()
        {
            FishingAreas = new MvxObservableCollection<FishingAreaModel>(
                        await DataStore.GetItemsAsync()
                );
        }

        #endregion
    }
}
