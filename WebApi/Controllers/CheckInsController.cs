using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("api/checkins")]
    public class CheckInsController : ApiController
    {
        // GET: api/CheckIns
        private MedStationContext db = new MedStationContext();

        [HttpGet(), Route("")]
        public async Task<IHttpActionResult> Get(DateTime? after = null, DateTime? before = null, int? previousId = null, bool isDone = false)
        {
            List<CheckIn> checkIns = await db.CheckIns.Include(x => x.Patient)
                .Where(x => x.AppointedTo.CompareTo(after ?? new DateTime(1, 1, 1)) >= 0 && x.AppointedTo.CompareTo(before ?? new DateTime(5000, 12, 31)) < 0 && x.IsDone == isDone && (previousId.HasValue ? previousId : -1) < x.Id)
                .ToListAsync();

            return Ok(Mapper.Map<List<CheckInDTO>>(checkIns));
        }

        [HttpGet(), Route("{id}")]
        public async Task<IHttpActionResult> Get(long id)
        {
            return Ok(await db.CheckIns.FirstOrDefaultAsync(x => x.Id == id));
        }

        // POST: api/CheckIns
        public void Post([FromBody]string value)
        {
        }


        /*
         *  TODO: Rethink this, probably create a different entity such as a medical history entry, 
         *  where you basically have the date, a text description of the health issue related, and whatever information
         *  It would be mandatory to have a post request to that, which would set the related checkin to "done".
         */
        // PUT: api/CheckIns/5
        [HttpPost(), Route("{id}/is-done")]
        public async Task<IHttpActionResult> FinishCheckIn(long id, HistoryEntryDTO historyEntryDTO)
        {
            //TODO: Add the checkin to the patient as a history entry in some sort of way. Done?
            CheckIn checkIn = await db.CheckIns.Include(x => x.Patient).Include(x => x.Patient.MedicalHistory).FirstOrDefaultAsync(x => x.Id == id);
            checkIn.IsDone = true;
            HistoryEntry historyEntry = new HistoryEntry()
            {
                Date = DateTime.Now,
                CheckInId = checkIn.Id,
                Description = historyEntryDTO.Description
            };
            if (checkIn.Patient.MedicalHistory == null)
                checkIn.Patient.MedicalHistory = new List<HistoryEntry>()
                {
                    historyEntry
                };
            else
                checkIn.Patient.MedicalHistory.Add(historyEntry);
            await db.SaveChangesAsync();
            return Ok();
        }

        [HttpPost(), Route("{id}/arrival")]
        public async Task<IHttpActionResult> PutArrival(long id)
        {
            CheckIn checkIn = await db.CheckIns.FirstOrDefaultAsync(x => x.Id == id);
            if (!checkIn.IsAppointment)
                return BadRequest();
            checkIn.Arrival = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return Ok(Mapper.Map<CheckInDTO>(checkIn));
        }

        // DELETE: api/CheckIns/5
        public void Delete(int id)
        {
        }
    }
}
