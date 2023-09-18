namespace Synergy.StandardApps.Worker.Utility.Notifications
{
    public interface INotifier<T>
    {
        void Notify(T entity);
    }
}
