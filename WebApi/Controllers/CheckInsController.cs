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
        private MedStationContext db = new MedStationContext();

        // GET: api/checkins
        [HttpGet(), Route("")]
        public async Task<IHttpActionResult> Get(DateTime? after = null, DateTime? before = null, int? previousId = null, bool isDone = false)
        {
            List<CheckIn> checkIns = await db.CheckIns.Include(x => x.Patient)
                .Where(x => x.AppointedTo.CompareTo(after ?? new DateTime(1, 1, 1)) >= 0 && x.AppointedTo.CompareTo(before ?? new DateTime(5000, 12, 31)) < 0 && x.IsDone == isDone && (previousId.HasValue ? previousId : -1) < x.Id)
                .ToListAsync();

            return Ok(Mapper.Map<List<CheckInDTO>>(checkIns));
        }

        // GET: api/checkins/id
        [HttpGet(), Route("{id}")]
        public async Task<IHttpActionResult> Get(long id)
        {
            return Ok(await db.CheckIns.FirstOrDefaultAsync(x => x.Id == id));
        }

        // POST: api/CheckIns/id/is-done
        [HttpPost(), Route("{id}/is-done")]
        public async Task<IHttpActionResult> FinishCheckIn(long id, HistoryEntryDTO historyEntryDTO)
        {
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
    }
}
