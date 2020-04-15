using SQLite;
using TrackYourTrip.Core.Helpers;
using TrackYourTrip.Core.Models;
using TrackYourTrip.Models;

namespace TrackYourTrip.Core.Services
{
    public static class DataServiceFactory
    {
        private static SQLiteConnection _connection;

        public static SQLiteConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    DatabaseHelper dbHelper = new DatabaseHelper();
                    _connection = dbHelper.GetConnection();
                }

                return _connection;
            }

            set => _connection = value;
        }

        public static SimpleDataService<SettingModel> GetSettingFactory()
        {
            return new SimpleDataService<SettingModel>(Connection, TableConsts.SETTINGS_TABLE);
        }

        public static SimpleDataService<GenerallSettingModel> GetGenerallSettingFactory()
        {
            return new SimpleDataService<GenerallSettingModel>(Connection, TableConsts.GENERALL_SETTING_TABLE);
        }

        public static SimpleDataService<FishingAreaModel> GetFishingAreaFactory()
        {
            return new SimpleDataService<FishingAreaModel>(Connection, TableConsts.FISHINGAREA_TABLE);
        }

        public static SimpleDataService<SpotModel> GetSpotFactory()
        {
            return new SimpleDataService<SpotModel>(Connection, TableConsts.SPOT_TABLE);
        }

        public static SimpleDataService<FishedSpotModel> GetFishedSpotFactory()
        {
            return new SimpleDataService<FishedSpotModel>(Connection, TableConsts.FISHEDSPOT_TABLE);
        }

        public static SimpleDataService<WaterModel> GetWaterFactory()
        {
            return new SimpleDataService<WaterModel>(Connection, TableConsts.WATERMODEL_TABLE);
        }

        public static SimpleDataService<TurbidityModel> GetTurbidityFactory()
        {
            return new SimpleDataService<TurbidityModel>(Connection, TableConsts.TURBIDITY_TABLE);
        }

        public static SimpleDataService<WaterColorModel> GetWaterColorFactory()
        {
            return new SimpleDataService<WaterColorModel>(Connection, TableConsts.WATERCOLOR_TABLE);
        }

        public static SimpleDataService<CurrentModel> GetCurrentFactory()
        {
            return new SimpleDataService<CurrentModel>(Connection, TableConsts.CURRENT_TABLE);
        }

        public static SimpleDataService<BaitColorModel> GetBaitColorFactory()
        {
            return new SimpleDataService<BaitColorModel>(Connection, TableConsts.BAITCOLOR_TABLE);
        }

        public static SimpleDataService<BaitTypeModel> GetBaitTypeFactory()
        {
            return new SimpleDataService<BaitTypeModel>(Connection, TableConsts.BAITTYPE_TABLE);
        }

        public static SimpleDataService<SpotTypeModel> GetSpotTypeFactory()
        {
            return new SimpleDataService<SpotTypeModel>(Connection, TableConsts.SPOTTYPE_TABLE);
        }

        public static SimpleDataService<TripModel> GetTripFactory()
        {
            return new SimpleDataService<TripModel>(Connection, TableConsts.TRIP_TABLE);
        }
    }
}
