namespace Synergy.StandardApps.Misc.Notifications
{
    public interface INotifier<T>
    {
        void Notify(T entity);
    }
}
