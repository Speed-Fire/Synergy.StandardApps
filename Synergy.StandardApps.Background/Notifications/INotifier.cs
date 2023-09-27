namespace Synergy.StandardApps.Background.Notifications
{
    public interface INotifier<T>
    {
        void Notify(T entity);
    }
}
