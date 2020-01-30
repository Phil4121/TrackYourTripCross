namespace TrackYourTrip.Core.Interfaces
{
    public interface IDependencyService
    {
        T Get<T>() where T : class;
    }
}
