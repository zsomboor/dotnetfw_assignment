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
    [RoutePrefix("api/patients")]
    public class PatientsController : ApiController
    {

        MedStationContext db = new MedStationContext(); 

        // GET: api/Patients
        [HttpGet(), Route("")]
        public async Task<IHttpActionResult> Get([FromUri]PatientFilterDTO filter)
        {
            var query = db.Patients as IQueryable<Patient>;

                if (filter?.SocialSecurityId != null)
                    query = query.Where(x => x.SocialSecurityId == filter.SocialSecurityId);
                else
                {
                    if (filter?.FirstName != null)
                        query = query.Where(x => x.FirstName.ToLower().Contains(filter.FirstName.ToLower()));
                    if (filter?.MiddleName != null)
                        query = query.Where(x => x.MiddleName.ToLower().Contains(filter.MiddleName.ToLower()));
                    if (filter?.LastName != null)
                        query = query.Where(x => x.LastName.ToLower().Contains(filter.LastName.ToLower()));
                }

            return Ok(Mapper.Map<List<PatientBaseDTO>>(await query.ToListAsync()));
        }

        // GET: api/Patients/5
        [HttpGet(), Route("{id}")]
        public async Task<IHttpActionResult> Get(long id)
        {
            Patient patient = await db.Patients.FirstOrDefaultAsync(x => x.Id == id);
            if (patient == null)
                return NotFound();
            return Ok(Mapper.Map<PatientBaseDTO>(patient));
        }

        // GET: api/Patients/5
        [HttpGet(), Route("{id}/medical-history")]
        public async Task<IHttpActionResult> GetHistory(long id)
        {
            Patient patient = await db.Patients.Include(x => x.MedicalHistory).FirstOrDefaultAsync(x => x.Id == id);
            if (patient == null)
                return NotFound();
            if (patient.MedicalHistory == null)
                Debug.WriteLine("Medical history is null.");
            return Ok(Mapper.Map<List<HistoryEntryDTO>>(patient.MedicalHistory ?? new List<HistoryEntry>()));
        }

        // POST: api/Patients
        [HttpPost(), Route("{id}/checkins")]
        public async Task<IHttpActionResult> AddNewCheckIn(long id, CheckInDTO checkInDto)
        {

            Patient patient = await db.Patients.FirstOrDefaultAsync(x => x.Id == id);
            CheckIn checkIn = new CheckIn();
            checkIn.IsAppointment = checkInDto.IsAppointment;
            checkIn.Arrival = DateTime.Now;
            checkIn.AppointedTo = checkInDto.AppointedTo ?? checkIn.Arrival ;
            checkIn.Description = checkInDto.Description;
            if (patient.CheckIns == null)
                patient.CheckIns = new List<CheckIn>() { checkIn };
            else
                patient.CheckIns.Add(checkIn);
            await db.SaveChangesAsync();
            return Ok(Mapper.Map<CheckInDTO>(checkIn));
        }

        [HttpPost(), Route("{id}/appointments")]
        public async Task<IHttpActionResult> AddNewAppointment(long id, AppointmentDTO appointment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Patient patient = await db.Patients.Include(x => x.CheckIns).FirstOrDefaultAsync(x => x.Id == id);
            CheckIn checkIn = new CheckIn
            {
                IsAppointment = true,
                Arrival = new DateTime(0),
                AppointedTo = appointment.AppointedTo
            };
            if (patient.CheckIns == null)
                patient.CheckIns = new List<CheckIn>() { checkIn };
            else
                patient.CheckIns.Add(checkIn);
            await db.SaveChangesAsync();
            return Ok(Mapper.Map<CheckInDTO>(checkIn));
        }

        [HttpPost(), Route("")]
        public async Task<IHttpActionResult> Post([FromBody]PatientBaseDTO payload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (db.Patients.FirstOrDefault(x => x.SocialSecurityId == payload.SocialSecurityId) != null)
                return BadRequest("Social Security ID (TAJ) is already registered.");
            try
            {
                Patient patient = Mapper.Map<Patient>(payload);
                patient.AddedAt = DateTime.UtcNow;
                await db.Patients.AddAsync(patient);
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                while(exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
                return BadRequest(exception.Message);
            }
            
        }

        // PUT: api/Patients/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Patients/5
        public void Delete(int id)
        {
        }
    }
}
