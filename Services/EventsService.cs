namespace Eventures.Web.Services
{
    using Eventures.Data;
    using Eventures.Domain;

    public class EventsService : IEventsService
    {
        private readonly EventuresDbContext context;

        public EventsService(EventuresDbContext context)
        {
            this.context = context;
        }

        public void CreateEvent(Event @event)
        {
            Event evnt = new Event
            {
                Name = @event.Name,
                Place = @event.Place,
                Start = @event.Start,
                End = @event.End,
                Total = @event.Total,
                PricePerTicket = @event.PricePerTicket
            };

            this.context.Add(evnt);
            this.context.SaveChanges();
        }
    }
}
