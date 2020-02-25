namespace Eventures.Web.Services
{
    using Eventures.Domain;
    public interface IEventsService
    {
        void CreateEvent(Event @event);
    }
}