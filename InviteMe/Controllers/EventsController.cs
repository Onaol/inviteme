using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InviteMe.Data;
using InviteMe.Models;
using InviteMe.Extensions;
using InviteMe.Models.ViewModels;
using FluentEmail.Core;
using InviteMe.MailService;

namespace InviteMe.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IEmailService _emailService;

        public EventsController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment, IEmailService emailService)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
            _emailService = emailService;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events
                .Where(_ => _.TenantId == User.GetUserId())                
                .Include(_ => _.EventCategory);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.Where(_ => _.TenantId == User.GetUserId())
                .Include(_ => _.EventCategory)
                .Include(_ => _.Invites)
                .ThenInclude(_ => _.Contact)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["EventCategoryId"] = new SelectList(_context.EventCategories.Where(_ => _.TenantId == User.GetUserId()), "Id", "Name");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,EventImage,EventCategoryId,EventDateTime,AddressFirstLine,AddressSecondLine,AddressThirdLine,PostCode,MapUrl")] EventViewModel @event)
        {                                    
            var eventtime = DateTime.SpecifyKind(@event.EventDateTime, DateTimeKind.Utc);
            var dbEvent = new Event { 
                Id = Guid.NewGuid(), 
                TenantId = User.GetUserId(),
                Name = @event.Name,
                Description = @event.Description,
                EventDateTime = eventtime,
                EventCategoryId = @event.EventCategoryId,
                CreatedDate = DateTime.UtcNow,
                PostCode = @event.PostCode,
                MapUrl = @event.MapUrl,
                AddressFirstLine = @event.AddressFirstLine,
                AddressSecondLine = @event.AddressSecondLine,
                AddressThirdLine = @event.AddressThirdLine
            };            

            if (@event.EventImage != null)
            {
                dbEvent.ImageUrl = UploadedFile(@event);
            }
            _context.Add(dbEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Checkin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkin(Guid Id)
        {

            var @invite = await _context.Invites.Include(_ => _.Contact).Include(_ => _.Event).SingleOrDefaultAsync(_ => _.Id == Id);
            if (@invite != null)
            {
                invite.Attended = true;
                invite.TimeCheckedIn = DateTime.UtcNow;                
                await _context.SaveChangesAsync();
            }                                              
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }


        [HttpPost, ActionName("InviteContacts")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteContacts(Guid EventId)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(EventId);
            if (@event != null)
            {
                var contacts = await _context.Contacts.Where(_ => _.TenantId == @event.TenantId).ToListAsync();

                if (contacts != null)
                {
                    foreach (var contact in contacts)
                    {
                        if (_context.Invites.Any(_ => _.EventId == @event.Id && _.ContactId == contact.Id))
                        {
                            continue;
                        }

                        _context.Invites.Add(new Invite
                        {
                            Id = Guid.NewGuid(),
                            ContactId = contact.Id,
                            EventId = @event.Id,
                            TenantId= @event.TenantId,
                            UniqueInviteKey = RandomString(6)
                        });
                    }
                }               
            }

            await _context.SaveChangesAsync();

            _context.Invites.Include(_ => _.Event).Include(_ => _.Contact).Where(_ => _.EventId == EventId).ForEach(_ => {
                _emailService.SendInvite(new EmailMetadata(_.Contact.Email, "You are Invited!"), $"InviteMe.Views.Shared.Invite.cshtml", _);
                });

            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(Guid id)
        {
          return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string UploadedFile(EventViewModel model)
        {
            string uniqueFileName = null;

            if (model.EventImage != null)
            {

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.EventImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.EventImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
