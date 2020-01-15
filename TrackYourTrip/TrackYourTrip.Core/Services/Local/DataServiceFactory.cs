﻿using SQLite;
using System.Collections.ObjectModel;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Interfaces;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Models;

namespace TrackYourTrip.Core.Services
{
    public static class DataServiceFactory
    {
        private static SQLiteConnection _connection;

        public static SQLiteConnection Connection {
            get
            {
                if (_connection == null)
                {
                    var dbHelper = new DatabaseHelper();
                    _connection = dbHelper.GetConnection();
                }

                return _connection;
            }

            set
            {
                _connection = value;
            }
        }

        public static SimpleDataService<SettingModel> GetSettingFactory()
        {
            return new SimpleDataService<SettingModel>(Connection, TableConsts.SETTINGS_TABLE);
        }

        public static SimpleDataService<FishingAreaModel> GetFishingAreaFactory()
        {
            return new SimpleDataService<FishingAreaModel>(Connection, TableConsts.FISHINGAREA_TABLE);
        }

        public static SimpleDataService<SpotModel> GetSpotFactory()
        {
            return new SimpleDataService<SpotModel>(Connection, TableConsts.SPOT_TABLE);
        }


        public static SimpleDataService<WaterModel> GetWaterFactory()
        {
            return new SimpleDataService<WaterModel>(Connection, TableConsts.WATERMODEL_TABLE);
        }

        public static SimpleDataService<SpotTypeModel> GetSpotTypeFactory()
        {
            return new SimpleDataService<SpotTypeModel>(Connection, TableConsts.SPOTTYPE_TABLE);
        }
    }
}