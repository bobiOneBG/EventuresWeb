namespace Eventures.Web.Controllers
{
    using Eventures.Data;
    using Eventures.Domain;
    using Eventures.Web.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventsController : Controller
    {
        private readonly EventuresDbContext context;
        private readonly IEventsService eventsService;

        public EventsController(EventuresDbContext context, IEventsService eventsService)
        {
            this.context = context;
            this.eventsService = eventsService;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return this.View(await this.context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Event @event = await this.context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return this.NotFound();
            }

            return this.View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties
        //you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Place,Start,End,Total,PricePerTicket")] Event @event)
        {
            if (this.ModelState.IsValid)
            {
                eventsService.CreateEvent(@event);
                
                return this.RedirectToAction(nameof(Index));
            }

            return this.View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Event @event = await this.context.Events.FindAsync(id);
            if (@event == null)
            {
                return this.NotFound();
            }
            return this.View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Place,Start,End,Total,PricePerTicket")] Event @event)
        {
            
            if (id != @event.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.context.Update(@event);
                    await this.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.EventExists(@event.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return this.RedirectToAction(nameof(Index));
            }
            return this.View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            Event @event = await this.context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return this.NotFound();
            }

            return this.View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Event @event = await this.context.Events.FindAsync(id);
            this.context.Events.Remove(@event);
            await this.context.SaveChangesAsync();
            return this.RedirectToAction(nameof(Index));
        }

        private bool EventExists(string id)
        {
            return this.context.Events.Any(e => e.Id == id);
        }
    }
}
