namespace Synergy.StandardApps.Worker.Utility.Notifications
{
    public interface INotificator<T>
    {
        void Notify(T entity);
    }
}
