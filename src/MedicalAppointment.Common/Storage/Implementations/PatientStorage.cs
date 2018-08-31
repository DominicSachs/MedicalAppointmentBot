using System;
using System.Threading.Tasks;
using MedicalAppointment.Common.Data;
using MedicalAppointment.Common.Entities;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointment.Common.Storage.Implementations
{
    public class PatientStorage : Storage<Patient>, IPatientStorage
    {
        public async Task<Patient> Get(string firstName, string lastName, DateTime birthDate)
        {
            using (var ctx = new AppDbContext())
            {
                return await ctx.Patients.SingleOrDefaultAsync(p => p.FirstName == firstName && p.LastName == lastName && p.BirthDate == birthDate);
            }
        }
    }
}