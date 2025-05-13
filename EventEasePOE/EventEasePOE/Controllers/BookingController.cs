using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EventEasePOE.Models;

namespace EventEasePOE.Controllers
{
    public class BookingController : Controller
    {
        private readonly EventEaseContext db = new EventEaseContext();

        // GET: Booking
        public ActionResult Index(string searchTerm)
        {
            var bookings = db.Bookings
                             .Include(b => b.Event)
                             .Include(b => b.Venue);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                bookings = bookings.Where(b =>
                    b.BookingId.ToString().Contains(searchTerm) ||
                    b.Event.EventName.Contains(searchTerm));
            }

            ViewBag.CurrentFilter = searchTerm;
            return View(bookings.ToList());
        }

        // GET: Booking/Details/5
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var booking = db.Bookings
                            .Include(b => b.Event)
                            .Include(b => b.Venue)
                            .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
                return HttpNotFound();

            return View(booking);
        }

        // GET: Booking/Create
        public ActionResult Create()
        {
            PopulateEventAndVenueSelectLists();
            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                bool isDoubleBooked = db.Bookings.Any(b =>
                    b.VenueId == booking.VenueId &&
                    DbFunctions.TruncateTime(b.BookingDate) == DbFunctions.TruncateTime(booking.BookingDate));

                if (isDoubleBooked)
                {
                    ModelState.AddModelError("", "This venue is already booked on the selected date.");
                }
                else
                {
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Booking created successfully.";
                    return RedirectToAction("Index");
                }
            }

            PopulateEventAndVenueSelectLists(booking.EventId, booking.VenueId);
            return View(booking);
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var booking = db.Bookings
                            .Include(b => b.Event)
                            .Include(b => b.Venue)
                            .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
                return HttpNotFound();

            PopulateEventAndVenueSelectLists(booking.EventId, booking.VenueId);
            return View(booking);
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,EventId,VenueId,BookingDate")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                bool isDoubleBooked = db.Bookings.Any(b =>
                    b.BookingId != booking.BookingId &&
                    b.VenueId == booking.VenueId &&
                    DbFunctions.TruncateTime(b.BookingDate) == DbFunctions.TruncateTime(booking.BookingDate));

                if (isDoubleBooked)
                {
                    ModelState.AddModelError("", "This venue is already booked on the selected date.");
                }
                else
                {
                    db.Entry(booking).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Booking updated successfully.";
                    return RedirectToAction("Index");
                }
            }

            PopulateEventAndVenueSelectLists(booking.EventId, booking.VenueId);
            return View(booking);
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var booking = db.Bookings
                            .Include(b => b.Event)
                            .Include(b => b.Venue)
                            .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
                return HttpNotFound();

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var booking = db.Bookings.Find(id);

            try
            {
                db.Bookings.Remove(booking);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Booking deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["DeleteError"] = "Booking can't be deleted due to related data constraints.";
                return RedirectToAction("Delete", new { id = id });
            }
        }

        private void PopulateEventAndVenueSelectLists(int? eventId = null, int? venueId = null)
        {
            ViewBag.EventId = new SelectList(db.Events, "EventId", "EventName", eventId);
            ViewBag.VenueId = new SelectList(db.Venues, "VenueId", "VenueName", venueId);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}