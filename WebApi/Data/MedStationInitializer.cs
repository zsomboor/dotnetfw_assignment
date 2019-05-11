using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebApi.Models;

namespace WebApi.Data
{
    public class MedStationInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<MedStationContext>
    {
        protected override void Seed(MedStationContext context)
        {
            var patients = new List<Patient>
            {
                new Patient("Patient 1", DateTime.UtcNow),
                new Patient("Patient 2"),
                new Patient("Patient 3"),
                new Patient("Patient 4"),
                new Patient("Patient 5"),
                new Patient("Patient 6"),
                new Patient("Patient 7")
            };

            patients.ForEach(p => context.Patients.Add(p));
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.InnerException.Message);
                Debug.WriteLine(ex.InnerException.InnerException.Message);
            }
        }
    }
}